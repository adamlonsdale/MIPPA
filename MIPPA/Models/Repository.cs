using Microsoft.EntityFrameworkCore;
using Mippa.Models;
using Mippa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mippa.ViewModels.Statistics;
using MIPPA.ViewModels;

namespace Mippa.Models
{
    public class Repository : IRepository
    {
        private readonly MippaContext _context;
        private ILogger<Repository> _logger;

        public Repository(MippaContext context, ILogger<Repository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void ResetScorecard(int scorecardId)
        {
            _context.RemoveRange(_context.TeamResults.Where(x => x.ScorecardId == scorecardId).ToList());
            _context.RemoveRange(_context.PlayerResults.Where(x => x.ScorecardId == scorecardId).ToList());
            _context.RemoveRange(_context.PlayerMatches.Where(x => x.ScorecardId == scorecardId).ToList());
            _context.Scorecards.Single(x => x.ScorecardId == scorecardId).State = 0;

            _context.SaveChanges();
        }

        public IEnumerable<ResetRequest> GetResetRequestsForSession(int sessionId)
        {
            var resetRequests =
                _context.ResetRequests.Include(x => x.Scorecard).ThenInclude(x => x.TeamMatch).ThenInclude(x => x.Schedule).Where(x => x.Scorecard.TeamMatch.Schedule.SessionId == sessionId);

            if (resetRequests == null)
            {
                return null;
            }

            return resetRequests.AsEnumerable();
        }

        public void RequestReset(int scorecardId, ResetRequest request)
        {
            if (_context.ResetRequests.Any(x => x.ScorecardId == scorecardId))
            {
                var requestFromContext = _context.ResetRequests.Single(x => x.ScorecardId == scorecardId);

                requestFromContext.Name = request.Name;
                requestFromContext.PhoneNumber = request.PhoneNumber;
                requestFromContext.Preference = request.Preference;
            }
            else
            {
                request.ResetRequestId = 0;
                _context.ResetRequests.Add(request);
            }

            _context.SaveChanges();

        }

        public IEnumerable<HandicapViewModel> GetHandicapViewModelsForSession(int sessionId)
        {
            var playerIds =
                _context.TeamRosters
                .Include(x => x.Team)
                .Include(x => x.Player)
                .Where(x => x.Team.SessionId == sessionId)
                .Select(x => x.PlayerId).Distinct();


            var playersInSession =
                GetAllPlayersFromSession(sessionId).Select(
                    x =>
                    new HandicapViewModel
                    {
                        Name = x.Player.Name,
                        PlayerId = x.PlayerId,
                        Handicap = x.Handicap
                    }
                    ).ToList();

            var playerStatisticsInSession =
                GetStatisticsForSession(sessionId, Format.EightBall);

            foreach (var playerStat in playerStatisticsInSession.PlayerStatistics)
            {
                var player = playersInSession.Single(x => x.PlayerId == playerStat.PlayerId);

                player.Plays = playerStat.PlayCount;
                player.ProjectedHandicap = playerStat.ProjectedHandicap.ToString("#.##");
            }

            return playersInSession.AsEnumerable();
        }

        public void FinalizeMatch(int scorecardId)
        {
            var scorecard = _context.Scorecards.SingleOrDefault(x => x.ScorecardId == scorecardId);

            if (scorecard != null)
            {
                scorecard.State = ScorecardState.Finalized;

                _context.SaveChanges();
            }
        }

        public LoginViewModel GetLoginViewModel(LoginViewModel viewModel)
        {
            var result = _context.Managers.SingleOrDefault(x => x.Email == viewModel.UserName && x.Password == viewModel.Password);

            if (result == null)
            {
                viewModel.Id = -1;
                viewModel.Valid = false;
                viewModel.Message = "Manager not found with e-mail address!";
            }
            else
            {
                viewModel.Id = result.ManagerId;
                viewModel.Type = "manager";
                viewModel.Valid = true;
            }

            return viewModel;
        }

        public void FinalizeAllScorecards()
        {
            HashSet<int> scorecardIds = new HashSet<int>();

            foreach (var scorecard in _context.Scorecards)
            {
                scorecardIds.Add(scorecard.ScorecardId);
            }

            foreach (var scorecardId in scorecardIds)
            {
                CalculateTeamResults(scorecardId, true);
                CalculatePlayerResults(scorecardId, true);

                var scorecard = _context.Scorecards.Single(x => x.ScorecardId == scorecardId);

                if (scorecard.State == ScorecardState.Completed)
                {
                    scorecard.State = ScorecardState.Finalized;
                }
            }

            _context.SaveChanges();
        }

        public StatisticsViewModel GetStatisticsForSession(int sessionId, Format format = Format.EightBall)
        {
            var session = _context.Sessions.SingleOrDefault(s => s.SessionId == sessionId);

            if (session == null)
            {
                return null;
            }

            var viewModel = new StatisticsViewModel();

            // Go through all TeamResults in session

            var sessionTeamResults =
                _context.TeamResults
                .Include(r => r.Scorecard)
                .ThenInclude(r => r.TeamMatch)
                .ThenInclude(r => r.Schedule)
                .ThenInclude(r => r.Session)
                .Include(r => r.Team)
                .Where(r => r.Scorecard.TeamMatch.Schedule.Session.SessionId == sessionId && r.Scorecard.Format == format);

            var sessionTeamResultsDictionary =
                new Dictionary<Team, TeamStatisticsViewModel>();

            foreach (var teamResult in sessionTeamResults)
            {
                if (sessionTeamResultsDictionary.ContainsKey(teamResult.Team))
                {
                    sessionTeamResultsDictionary[teamResult.Team].TotalScore += teamResult.Score;
                    sessionTeamResultsDictionary[teamResult.Team].TotalWins += teamResult.Wins;
                }
                else
                {
                    sessionTeamResultsDictionary.Add(
                        teamResult.Team,
                        new TeamStatisticsViewModel
                        {
                            Name = teamResult.Team.Name,
                            TotalScore = teamResult.Score,
                            TotalWins = teamResult.Wins
                        });
                }
            }

            var sortedTeamResults =
                sessionTeamResultsDictionary
                .OrderByDescending(r => r.Value.TotalWins)
                .ThenByDescending(r => r.Value.TotalScore)
                .Select(r => r.Value)
                .ToList();

            viewModel.TeamStatistics = sortedTeamResults;

            var sessionPlayerResults =
                _context.PlayerResults
                .Include(r => r.Scorecard)
                .ThenInclude(r => r.TeamMatch)
                .ThenInclude(r => r.Schedule)
                .ThenInclude(r => r.Session)
                .Include(r => r.Player)
                .Where(r => r.Scorecard.TeamMatch.Schedule.Session.SessionId == sessionId && r.Scorecard.Format == format);

            var sessionPlayerResultsDictionary =
                new Dictionary<Player, PlayerStatisticsViewModel>();

            foreach (var playerResult in sessionPlayerResults)
            {
                if (sessionPlayerResultsDictionary.ContainsKey(playerResult.Player))
                {
                    sessionPlayerResultsDictionary[playerResult.Player].TotalScore += playerResult.Score;
                    sessionPlayerResultsDictionary[playerResult.Player].TotalWins += playerResult.Wins;
                    sessionPlayerResultsDictionary[playerResult.Player].PlayCount ++;
                }
                else
                {
                    sessionPlayerResultsDictionary.Add(
                        playerResult.Player,
                        new PlayerStatisticsViewModel
                        {
                            Name = playerResult.Player.Name,
                            PlayerId = playerResult.PlayerId,
                            TotalScore = playerResult.Score,
                            TotalWins = playerResult.Wins,
                            PlayCount = playerResult.PlayCount,
                            ProjectedHandicap = 0.0
                        });
                }
            }

            foreach (var result in sessionPlayerResultsDictionary.Values)
            {
                result.ProjectedHandicap = result.TotalScore / (double)result.PlayCount;
                result.ProjectedHandicap = result.ProjectedHandicap /
                    (session.MatchupType == MatchupType.FiveOnFive ? 5.0 :
                    session.MatchupType == MatchupType.FiveOnFour ? 5.0 :
                    session.MatchupType == MatchupType.FourOnFour ? 4.0 :
                    3.0);
            }

            var sortedPlayerResults =
                sessionPlayerResultsDictionary
                .OrderByDescending(r => r.Value.TotalWins)
                .ThenByDescending(r => r.Value.TotalScore)
                .Select(r => r.Value)
                .ToList();

            viewModel.PlayerStatistics = sortedPlayerResults;



            return viewModel;
        }

        public PlayerResultsViewModel CalculatePlayerResults(int scorecardId, bool saveResults = false)
        {
            var scorecard = _context.Scorecards
                .Include(s => s.PlayerResults)
                .ThenInclude(s => s.Player)
                .Include(s => s.PlayerMatches)
                .ThenInclude(s => s.HomePlayerScore)
                .Include(s => s.PlayerMatches)
                .ThenInclude(s => s.AwayPlayerScore)
                .Include(s => s.TeamMatch)
                .ThenInclude(s => s.Schedule)
                .ThenInclude(s => s.Session)
                .SingleOrDefault(x => x.ScorecardId == scorecardId);

            PlayerResultsViewModel playerResultsViewModel = new PlayerResultsViewModel();

            if (scorecard == null)
            {
                return playerResultsViewModel;
            }

            if (scorecard.State == ScorecardState.Initial)
            {
                var homeTeam = _context.TeamRosters.Where(x => x.TeamId == scorecard.TeamMatch.HomeTeamId).Include(x => x.Player);
                var awayTeam = _context.TeamRosters.Where(x => x.TeamId == scorecard.TeamMatch.AwayTeamId).Include(x => x.Player);

                foreach (var player in homeTeam)
                {
                    playerResultsViewModel.HomePlayerResults.Add(new PlayerViewModel
                    {
                        PlayerId = player.PlayerId,
                        Name = player.Player.Name,
                        Handicap = ConvertHandicap(scorecard.Format, player.Handicap)
                    });
                }

                foreach (var player in awayTeam)
                {
                    playerResultsViewModel.AwayPlayerResults.Add(new PlayerViewModel
                    {
                        PlayerId = player.PlayerId,
                        Name = player.Player.Name,
                        Handicap = ConvertHandicap(scorecard.Format, player.Handicap)
                    });
                }

                return playerResultsViewModel;
            }

            int rounds = 5;

            if (scorecard.TeamMatch.Schedule.Session.MatchupType == MatchupType.FourOnFour)
            {
                rounds = 4;
            }
            else if (scorecard.TeamMatch.Schedule.Session.MatchupType == MatchupType.ThreeOnThree)
            {
                rounds = 3;
            }

            Dictionary<Player, int> scoresByPlayer = new Dictionary<Player, int>();
            Dictionary<Player, int> winsByPlayer = new Dictionary<Player, int>();

            var playerMatchesList = scorecard.PlayerMatches.ToList();

            // Iterate over all PlayerScores
            foreach (var match in playerMatchesList)
            {
                // Get home and away player
                var homePlayer = match.HomePlayer;
                var awayPlayer = match.AwayPlayer;

                // TODO: need to detect playback
                // keep track of rounds??

                if (!scoresByPlayer.ContainsKey(homePlayer))
                {
                    var playerResult = scorecard.PlayerResults.Single(p => p.PlayerId == match.HomePlayerId);
                    scoresByPlayer.Add(homePlayer, match.HomePlayerScore.Score);
                    playerResultsViewModel.HomePlayerResults.Add(new PlayerViewModel
                    {
                        PlayerId = match.HomePlayerId,
                        Name = match.HomePlayer.Name,
                        Handicap = playerResult.Handicap,
                        TotalScore = match.HomePlayerScore.Score,
                        Wins = 0
                    });
                }
                else
                {
                    var viewModel = playerResultsViewModel.HomePlayerResults.Single(x => x.PlayerId == match.HomePlayerId);
                    scoresByPlayer[homePlayer] += match.HomePlayerScore.Score;
                    viewModel.TotalScore += match.HomePlayerScore.Score;
                }

                if (!scoresByPlayer.ContainsKey(awayPlayer))
                {
                    var playerResult = scorecard.PlayerResults.Single(p => p.PlayerId == match.AwayPlayerId);
                    scoresByPlayer.Add(awayPlayer, match.AwayPlayerScore.Score);
                    playerResultsViewModel.AwayPlayerResults.Add(new PlayerViewModel
                    {
                        PlayerId = match.AwayPlayerId,
                        Name = match.AwayPlayer.Name,
                        Handicap = playerResult.Handicap,
                        TotalScore = match.AwayPlayerScore.Score,
                        Wins = 0
                    });
                }
                else
                {
                    var viewModel = playerResultsViewModel.AwayPlayerResults.Single(x => x.PlayerId == match.AwayPlayerId);
                    scoresByPlayer[awayPlayer] += match.AwayPlayerScore.Score;
                    viewModel.TotalScore += match.AwayPlayerScore.Score;
                }

                int homePlayerScore = match.HomePlayerScore.Score;
                int awayPlayerScore = match.AwayPlayerScore.Score;


                if (homePlayerScore > awayPlayerScore)
                {
                    var viewModel = playerResultsViewModel.HomePlayerResults.Single(x => x.PlayerId == match.HomePlayerId);

                    if (!winsByPlayer.ContainsKey(homePlayer))
                    {
                        winsByPlayer.Add(homePlayer, 1);
                    }
                    else
                    {
                        winsByPlayer[homePlayer]++;
                    }

                    viewModel.Wins = winsByPlayer[homePlayer];
                }
                else if (homePlayerScore < awayPlayerScore)
                {
                    var viewModel = playerResultsViewModel.AwayPlayerResults.Single(x => x.PlayerId == match.AwayPlayerId);

                    if (!winsByPlayer.ContainsKey(awayPlayer))
                    {
                        winsByPlayer.Add(awayPlayer, 1);
                    }
                    else
                    {
                        winsByPlayer[awayPlayer]++;
                    }

                    viewModel.Wins = winsByPlayer[awayPlayer];
                }
            }

            //if (saveResults)
            {
                // After going through each match, save the totals for each player
                foreach (var scoreByPlayer in scoresByPlayer)
                {
                    var viewModel = 
                        scorecard.PlayerMatches.Any(x => x.HomePlayerId == scoreByPlayer.Key.PlayerId) ?
                        playerResultsViewModel.HomePlayerResults.Single(x => x.PlayerId == scoreByPlayer.Key.PlayerId) :
                        playerResultsViewModel.AwayPlayerResults.Single(x => x.PlayerId == scoreByPlayer.Key.PlayerId);

                    // get play count
                    var playerResult = scorecard.PlayerResults.Single(p => p.PlayerId == scoreByPlayer.Key.PlayerId);

                    var averageScore = scoreByPlayer.Value / playerResult.PlayCount;
                    viewModel.AverageScore = averageScore;

                    if (saveResults) playerResult.Score = averageScore;
                }

                foreach (var winByPlayer in winsByPlayer)
                {
                    var viewModel =
                        scorecard.PlayerMatches.Any(x => x.HomePlayerId == winByPlayer.Key.PlayerId) ?
                        playerResultsViewModel.HomePlayerResults.Single(x => x.PlayerId == winByPlayer.Key.PlayerId) :
                        playerResultsViewModel.AwayPlayerResults.Single(x => x.PlayerId == winByPlayer.Key.PlayerId);

                    // get play count
                    var playerResult = scorecard.PlayerResults.Single(p => p.PlayerId == winByPlayer.Key.PlayerId);

                    var averageWins = winByPlayer.Value / playerResult.PlayCount;

                    if (saveResults) playerResult.Wins = averageWins;
                }

                if (saveResults) _context.SaveChanges();
            }

            return playerResultsViewModel;
        }

        public TeamResultsViewModel CalculateTeamResults(int scorecardId, bool saveResults)
        {
            var scorecard = _context.Scorecards
                .Include(x => x.PlayerMatches)
                .ThenInclude(x => x.HomePlayerScore)
                .Include(x => x.PlayerMatches)
                .ThenInclude(x => x.HomePlayer)
                .Include(x => x.PlayerMatches)
                .ThenInclude(x => x.AwayPlayerScore)
                .Include(x => x.PlayerMatches)
                .ThenInclude(x => x.AwayPlayer)
                .Include(x => x.PlayerResults)
                .Include(x => x.TeamMatch)
                .ThenInclude(x => x.Schedule)
                .ThenInclude(x => x.Session)
                .Include(x => x.TeamResults)
                .ThenInclude(x => x.Team)
                .SingleOrDefault(x => x.ScorecardId == scorecardId);

            int rounds = 0;
            int matchesPerRound = 0;

            if (scorecard.State == ScorecardState.Initial)
            {
                return new TeamResultsViewModel();
            }

            List<double> homeTeamRoundsWon = new List<double>();
            List<double> awayTeamRoundsWon = new List<double>();

            switch (scorecard.TeamMatch.Schedule.Session.MatchupType)
            {
                case MatchupType.FiveOnFive:
                    rounds = 5;
                    matchesPerRound = 5;
                    homeTeamRoundsWon = new List<double>() { 0.0, 0.0, 0.0, 0.0, 0.0 };
                    awayTeamRoundsWon = new List<double>() { 0.0, 0.0, 0.0, 0.0, 0.0 };
                    break;
                case MatchupType.FiveOnFour:
                    rounds = 4;
                    matchesPerRound = 5;
                    homeTeamRoundsWon = new List<double>() { 0.0, 0.0, 0.0, 0.0, 0.0 };
                    awayTeamRoundsWon = new List<double>() { 0.0, 0.0, 0.0, 0.0, 0.0 };
                    break;
                case MatchupType.FourOnFour:
                    rounds = 4;
                    matchesPerRound = 4;
                    homeTeamRoundsWon = new List<double>() { 0.0, 0.0, 0.0, 0.0 };
                    awayTeamRoundsWon = new List<double>() { 0.0, 0.0, 0.0, 0.0 };
                    break;
                case MatchupType.ThreeOnThree:
                    rounds = 3;
                    matchesPerRound = 3;
                    homeTeamRoundsWon = new List<double>() { 0.0, 0.0, 0.0 };
                    awayTeamRoundsWon = new List<double>() { 0.0, 0.0, 0.0 };
                    break;
            }

            int homeTeamHandicap = 0;
            int awayTeamHandicap = 0;

            for (int i = 0; i < rounds; i++)
            {
                var match = scorecard.PlayerMatches.ElementAt(i);

                var homePlayerHandicap = scorecard.PlayerResults.Single(x => x.PlayerId == match.HomePlayer.PlayerId).Handicap;
                var awayPlayerHandicap = scorecard.PlayerResults.Single(x => x.PlayerId == match.AwayPlayer.PlayerId).Handicap;

                homeTeamHandicap += homePlayerHandicap;
                awayTeamHandicap += awayPlayerHandicap;
            }

            if (homeTeamHandicap > awayTeamHandicap)
            {
                int difference = homeTeamHandicap - awayTeamHandicap;

                if (scorecard.Format == Format.NineBall)
                {
                    awayTeamHandicap = CalculateHandicap(difference);
                }
                else
                {
                    awayTeamHandicap = difference;
                }

                homeTeamHandicap = 0;
            }
            else
            {
                int difference = awayTeamHandicap - homeTeamHandicap;

                if (scorecard.Format == Format.NineBall)
                {
                    homeTeamHandicap = CalculateHandicap(difference);
                }
                else
                {
                    homeTeamHandicap = difference;
                }

                awayTeamHandicap = 0;
            }

            int homeTeamPointsTotal = 0;
            double homeTeamRoundsTotal = 0.0;
            int awayTeamPointsTotal = 0;
            double awayTeamRoundsTotal = 0.0;

            int count = 1;
            int currentRound = 1;
            bool roundComplete = true;
            int currentRoundHomeScore = 0;
            int currentRoundAwayScore = 0;

            foreach (var match in scorecard.PlayerMatches)
            {
                var homePlayerScore = match.HomePlayerScore.Score;
                var awayPlayerScore = match.AwayPlayerScore.Score;

                homeTeamPointsTotal += homePlayerScore;
                currentRoundHomeScore += homePlayerScore;

                awayTeamPointsTotal += awayPlayerScore;
                currentRoundAwayScore += awayPlayerScore;

                if (!match.Saved)
                    roundComplete = false;


                if (count > 0 &&
                    count % matchesPerRound == 0 &&
                    roundComplete)
                {
                    currentRoundHomeScore += scorecard.Format == Format.EightBall ? homeTeamHandicap : 0;
                    currentRoundAwayScore += scorecard.Format == Format.EightBall ? awayTeamHandicap : 0;
                    homeTeamPointsTotal += scorecard.Format == Format.EightBall ? homeTeamHandicap : 0;
                    awayTeamPointsTotal += scorecard.Format == Format.EightBall ? awayTeamHandicap : 0;

                    if (scorecard.Format == Format.EightBall &&
                        currentRoundHomeScore > currentRoundAwayScore)
                    {
                        homeTeamRoundsTotal++;
                        homeTeamRoundsWon[currentRound - 1] = 1.0;
                        awayTeamRoundsWon[currentRound - 1] = 0.0;
                    }
                    else if (scorecard.Format == Format.EightBall &&
                        currentRoundAwayScore > currentRoundHomeScore)
                    {
                        awayTeamRoundsTotal++;
                        homeTeamRoundsWon[currentRound - 1] = 0.0;
                        awayTeamRoundsWon[currentRound - 1] = 1.0;
                    }
                    else if (scorecard.Format == Format.EightBall)
                    {
                        homeTeamRoundsTotal += .5;
                        awayTeamRoundsTotal += .5;

                        homeTeamRoundsWon[currentRound - 1] = 0.5;
                        awayTeamRoundsWon[currentRound - 1] = 0.5;
                    }

                    currentRoundHomeScore = 0;
                    currentRoundAwayScore = 0;
                    currentRound++;
                    roundComplete = true;
                }

                count++;
            }

            if (!scorecard.PlayerMatches.Any(m => m.Saved == false))
            {
                if (scorecard.Format == Format.NineBall)
                {
                    homeTeamPointsTotal += homeTeamHandicap;
                    awayTeamPointsTotal += awayTeamHandicap;
                }
                else
                {
                    if (homeTeamPointsTotal > awayTeamPointsTotal)
                    {
                        homeTeamRoundsTotal++;
                    }
                    else if (awayTeamPointsTotal > homeTeamPointsTotal)
                    {
                        awayTeamRoundsTotal++;
                    }
                    else
                    {
                        homeTeamRoundsTotal += .5;
                        awayTeamRoundsTotal += .5;
                    }
                }
            }

            var teamMatch = new TeamResultsViewModel
            {
                AwayTeamHandicap = awayTeamHandicap,
                AwayTeamTotalRounds = awayTeamRoundsTotal,
                AwayTeamTotalPoints = awayTeamPointsTotal,
                HomeTeamHandicap = homeTeamHandicap,
                HomeTeamTotalPoints = homeTeamPointsTotal,
                HomeTeamTotalRounds = homeTeamRoundsTotal,
                HomeRoundsWon = homeTeamRoundsWon,
                AwayRoundsWon = awayTeamRoundsWon
            };

            if (saveResults)
            {
                var homeTeamResult =
                    _context.TeamResults.Any(r => r.ScorecardId == scorecardId && r.TeamId == scorecard.TeamMatch.HomeTeamId) ?
                    _context.TeamResults.Single(r => r.ScorecardId == scorecardId && r.TeamId == scorecard.TeamMatch.HomeTeamId) :
                    new TeamResult();
                var awayTeamResult =
                    _context.TeamResults.Any(r => r.ScorecardId == scorecardId && r.TeamId == scorecard.TeamMatch.AwayTeamId) ?
                    _context.TeamResults.Single(r => r.ScorecardId == scorecardId && r.TeamId == scorecard.TeamMatch.AwayTeamId) :
                    new TeamResult();

                homeTeamResult.Score = teamMatch.HomeTeamTotalPoints;
                homeTeamResult.Wins = scorecard.Format == Format.EightBall ? teamMatch.HomeTeamTotalRounds : teamMatch.HomeTeamTotalPoints;

                awayTeamResult.Score = teamMatch.AwayTeamTotalPoints;
                awayTeamResult.Wins = scorecard.Format == Format.EightBall ? teamMatch.AwayTeamTotalRounds : teamMatch.AwayTeamTotalPoints;

                if (homeTeamResult.TeamResultId <= 0)
                {
                    homeTeamResult.ScorecardId = scorecard.ScorecardId;
                    homeTeamResult.TeamId = scorecard.TeamMatch.HomeTeamId;
                    _context.TeamResults.Add(homeTeamResult);
                }

                if (awayTeamResult.TeamResultId <= 0)
                {
                    awayTeamResult.ScorecardId = scorecard.ScorecardId;
                    awayTeamResult.TeamId = scorecard.TeamMatch.AwayTeamId;
                    _context.TeamResults.Add(awayTeamResult);
                }

                _context.SaveChanges();
            }

            // Check if all scores are greater than zero
            if (!scorecard.PlayerMatches.Any(x=>x.Saved == false) && scorecard.State == ScorecardState.InProgress)
            {
                scorecard.State = ScorecardState.Completed;
                _context.SaveChanges();
            }

            teamMatch.ScorecardState = scorecard.State;


            return teamMatch;
        }

        private int CalculateHandicap(int difference)
        {
            switch (difference)
            {
                case 0:
                    return 0;
                case 1:
                case 2:
                    return 1;
                case 3:
                case 4:
                    return 2;
                case 5:
                case 6:
                    return 3;
                case 7:
                case 8:
                    return 4;
                default:
                    return 5;
            }
        }

        public ScorecardViewModel GetScorecardInformation(int scorecardId)
        {
            var scorecard = _context.Scorecards.SingleOrDefault(x => x.ScorecardId == scorecardId);

            if (scorecard == null)
            {
                return null;
            }

            switch (scorecard.State)
            {
                case ScorecardState.Initial:
                    return GetScorecardInformation_Initial(scorecardId);
                case ScorecardState.InProgress:
                case ScorecardState.Completed:
                case ScorecardState.Finalized:
                    return GetScorecardInformation_NotInitial(scorecardId);
                default:
                    return null;
            }
        }

        public ScorecardViewModel GetScorecardInformation_Initial(int scorecardId)
        {
            var scorecard =
                _context.Scorecards
                .Include(x => x.TeamMatch)
                .ThenInclude(x=>x.Scorecards)
                .Include(x => x.TeamMatch)
                .ThenInclude(x => x.HomeTeam)
                .ThenInclude(x => x.Players)
                .ThenInclude(x => x.Player)
                .Include(x => x.TeamMatch)
                .ThenInclude(x => x.AwayTeam)
                .ThenInclude(x => x.Players)
                .ThenInclude(x => x.Player)
                .Include(x => x.TeamMatch)
                .ThenInclude(x => x.Schedule)
                .ThenInclude(x => x.Session)
                .SingleOrDefault(x => x.ScorecardId == scorecardId);

            if (scorecard == null)
            {
                return null;
            }

            var viewModel = new ScorecardViewModel();

            viewModel.MatchupType = scorecard.TeamMatch.Schedule.Session.MatchupType;

            viewModel.OtherScorecardId = GetOtherScorecardId(scorecard);

            viewModel.ScorecardId = scorecard.ScorecardId;
            viewModel.HomeTeamId = scorecard.TeamMatch.HomeTeamId;
            viewModel.HomeTeamName = scorecard.TeamMatch.HomeTeam.Name;
            viewModel.AwayTeamId = scorecard.TeamMatch.AwayTeamId;
            viewModel.AwayTeamName = scorecard.TeamMatch.AwayTeam.Name;

            viewModel.HomePlayers = new HashSet<PlayerViewModel>();
            viewModel.AwayPlayers = new HashSet<PlayerViewModel>();

            viewModel.State = scorecard.State;
            viewModel.Format = scorecard.Format;

            foreach (var player in scorecard.TeamMatch.HomeTeam.Players)
            {
                viewModel.HomePlayers.Add(
                    new PlayerViewModel
                    {
                        Name = player.Player.Name,
                        Handicap = ConvertHandicap(viewModel.Format, player.Handicap),
                        PlayerId = player.PlayerId
                    });
            }

            foreach (var player in scorecard.TeamMatch.AwayTeam.Players)
            {
                viewModel.AwayPlayers.Add(
                    new PlayerViewModel
                    {
                        Name = player.Player.Name,
                        Handicap = ConvertHandicap(viewModel.Format, player.Handicap),
                        PlayerId = player.PlayerId
                    });
            }

            viewModel.NumberOfTables = scorecard.NumberOfTables;

            return viewModel;
        }

        private int GetOtherScorecardId(Scorecard scorecard)
        {
            if (scorecard.TeamMatch.Scorecards.Count > 1)
            {
                var otherScorecard = scorecard.TeamMatch.Scorecards.Single(x => x.ScorecardId != scorecard.ScorecardId);

                return otherScorecard.ScorecardId;
            }

            return -1;
        }

        private int ConvertHandicap(Format format, int handicap)
        {
            if (format == Format.EightBall)
            {
                return handicap;
            }

            switch (handicap)
            {
                case 4:
                case 5:
                    return -3;
                case 11:
                case 12:
                    return 3;
                default:
                    return handicap - 8;
            }
        }

        public ScorecardViewModel GetScorecardInformation_NotInitial(int scorecardId)
        {
            var scorecard =
                _context.Scorecards
                .Include(x => x.TeamMatch)
                .ThenInclude(x => x.Scorecards)
                .Include(x => x.TeamMatch)
                .ThenInclude(x => x.HomeTeam)
                .ThenInclude(x => x.Players)
                .ThenInclude(x => x.Player)
                .Include(x => x.TeamMatch)
                .ThenInclude(x => x.AwayTeam)
                .ThenInclude(x => x.Players)
                .ThenInclude(x => x.Player)
                .Include(x => x.PlayerMatches)
                .ThenInclude(x => x.HomePlayerScore)
                .Include(x => x.PlayerMatches)
                .ThenInclude(x => x.AwayPlayerScore)
                .Include(x => x.TeamMatch)
                .ThenInclude(x => x.Schedule)
                .ThenInclude(x => x.Session)
                .SingleOrDefault(x => x.ScorecardId == scorecardId);

            if (scorecard == null)
            {
                return null;
            }

            var viewModel = new ScorecardViewModel();

            viewModel.OtherScorecardId = GetOtherScorecardId(scorecard);
            viewModel.State = scorecard.State;
            viewModel.ScorecardId = scorecard.ScorecardId;
            viewModel.ScorecardId = scorecard.ScorecardId;
            viewModel.HomeTeamId = scorecard.TeamMatch.HomeTeamId;
            viewModel.HomeTeamName = scorecard.TeamMatch.HomeTeam.Name;
            viewModel.AwayTeamId = scorecard.TeamMatch.AwayTeamId;
            viewModel.AwayTeamName = scorecard.TeamMatch.AwayTeam.Name;
            viewModel.Format = scorecard.Format;
            viewModel.MatchupType = scorecard.TeamMatch.Schedule.Session.MatchupType;

            viewModel.Rounds = new HashSet<RoundViewModel>();

            int numberOfRounds = 0;
            int matchesPerRound = 0;

            List<int> breakingSchedule = new List<int>();

            switch (scorecard.TeamMatch.Schedule.Session.MatchupType)
            {
                case MatchupType.FiveOnFive:
                    breakingSchedule = new List<int> { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1 };

                    numberOfRounds = 5;
                    matchesPerRound = 5;
                    break;
                case MatchupType.FiveOnFour:
                    breakingSchedule = new List<int> { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };

                    numberOfRounds = 4;
                    matchesPerRound = 5;
                    break;
                case MatchupType.FourOnFour:
                    breakingSchedule = new List<int> { 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 };

                    numberOfRounds = 4;
                    matchesPerRound = 4;
                    break;
                case MatchupType.ThreeOnThree:
                    breakingSchedule = new List<int> { 1, 1, 1, 0, 0, 0, 1, 0, 1 };

                    numberOfRounds = 3;
                    matchesPerRound = 3;
                    break;
                default:
                    break;

            }

            int matchCounter = 0;

            viewModel.Rounds = new HashSet<RoundViewModel>();

            for (int i = 1; i <= numberOfRounds; i++)
            {
                var roundViewModel = new RoundViewModel();
                viewModel.Rounds.Add(roundViewModel);

                roundViewModel.PlayerMatches = new HashSet<PlayerMatchViewModel>();

                for (int j = 1; j <= matchesPerRound; j++)
                {
                    var playerMatchViewModel = new PlayerMatchViewModel();
                    roundViewModel.PlayerMatches.Add(playerMatchViewModel);

                    var match = scorecard.PlayerMatches.ElementAt(matchCounter);

                    playerMatchViewModel.HomePlayer =
                        new PlayerViewModel
                        {
                            PlayerId = match.HomePlayer.PlayerId,
                            Name = match.HomePlayer.Name,
                            Handicap = match.Scorecard.TeamMatch.HomeTeam.Players.Single(x => x.PlayerId == match.HomePlayer.PlayerId).Handicap
                        };
                    playerMatchViewModel.HomePlayerScore = match.HomePlayerScore.Score;
                    playerMatchViewModel.AwayPlayer =
                        new PlayerViewModel
                        {
                            PlayerId = match.AwayPlayer.PlayerId,
                            Name = match.AwayPlayer.Name,
                            Handicap = match.Scorecard.TeamMatch.AwayTeam.Players.Single(x => x.PlayerId == match.AwayPlayer.PlayerId).Handicap
                        };
                    playerMatchViewModel.AwayPlayerScore = match.AwayPlayerScore.Score;
                    playerMatchViewModel.PlayerMatchId = match.PlayerMatchId;
                    playerMatchViewModel.HomeBreaking = Convert.ToBoolean(breakingSchedule[matchCounter]);
                    playerMatchViewModel.Saved = match.Saved;
                    matchCounter++;
                }
            }

            viewModel.NumberOfTables = scorecard.NumberOfTables;

            return viewModel;
        }

        public void AddManager(Manager manager)
        {
            _context.Managers.Add(manager);
            _context.SaveChanges();
        }

        public void AddPlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        public void AddTeamToSession(Team team, int sessionId)
        {
            var session = _context.Sessions.SingleOrDefault(x => x.SessionId == sessionId);

            if (session == null)
            {
                return;
            }

            _context.Teams.Add(
                new Team
                {
                    Name = team.Name,
                    SessionId = sessionId
                }
                );
            _context.SaveChanges();

            var teamAdded = _context.Teams.Last();

            foreach (var player in team.Players)
                _context.TeamRosters.Add(
                    new TeamRoster
                    {
                        PlayerId = player.PlayerId,
                        TeamId = teamAdded.TeamId
                    }
                    );

            _context.SaveChanges();
        }

        public IEnumerable<Manager> GetAllManagers()
        {
            var managers = _context.Managers;

            return managers.AsEnumerable();
        }

        public IEnumerable<Team> GetAllTeams()
        {
            var result = _context.Teams
                .Include(x => x.Players)
                .ThenInclude(z => z.Player)
                .AsEnumerable();

            return result;
        }

        public IEnumerable<Team> GetAllTeamsFromSession(int sessionId)
        {
            var session =
                _context.Sessions
                .Include(x => x.Teams)
                .ThenInclude(y => y.Players)
                .ThenInclude(z => z.Player)
                .SingleOrDefault(x => x.SessionId == sessionId);

            if (session == null)
            {
                return null;
            }

            return session.Teams.AsEnumerable();
        }

        public Manager GetManager(int managerId)
        {
            return _context.Managers.SingleOrDefault(x => x.ManagerId == managerId);
        }

        public Player GetPlayer(int playerId)
        {
            return _context.Players.SingleOrDefault(x => x.PlayerId == playerId);
        }

        public Team GetTeamFromSessionByName(string name, int sessionId)
        {
            var session = _context.Sessions.Include(x => x.Teams).SingleOrDefault(x => x.SessionId == sessionId);

            if (session == null)
            {
                return null;
            }

            return session.Teams.SingleOrDefault(x => x.Name == name);
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _context.Players.AsEnumerable();
        }

        public IEnumerable<Player> GetPlayersFromQuery(string aQuery)
        {
            return _context.Players.Where(x => x.Name.Contains(aQuery)).AsEnumerable();
        }

        public Manager RemoveManager(int managerId)
        {
            var managerFromContext = _context.Managers.SingleOrDefault(x => x.ManagerId == managerId);

            if (managerFromContext == null)
            {
                return null;
            }

            _context.Managers.Remove(managerFromContext);
            _context.SaveChanges();

            return managerFromContext;
        }

        public Player RemovePlayer(int playerId)
        {
            var playerFromContext = _context.Players.SingleOrDefault(x => x.PlayerId == playerId);

            if (playerFromContext == null)
            {
                return null;
            }

            _context.Players.Remove(playerFromContext);
            _context.SaveChanges();

            return playerFromContext;
        }

        public Team RemoveTeam(int teamId)
        {
            throw new NotImplementedException();
        }

        public void UpdateManager(Manager manager)
        {
            var managerFromContext = _context.Managers.SingleOrDefault(x => x.ManagerId == manager.ManagerId);

            if (managerFromContext == null)
            {
                return;
            }

            managerFromContext.Name = manager.Name;

            _context.SaveChanges();
        }

        public void UpdatePlayer(Player player)
        {
            var playerFromContext = _context.Players.SingleOrDefault(x => x.PlayerId == player.PlayerId);

            if (playerFromContext == null)
            {
                return;
            }

            playerFromContext.Name = player.Name;

            _context.SaveChanges();
        }

        public void UpdateTeam(Team team)
        {
            _logger.LogInformation("Updating TeamId: {0}...TeamName: {1}...NumPlayers: {2}",
                team.TeamId,
                team.Name,
                team.Players.Count
                );

            var teamFromContext =
                _context.Teams
                .Include(x => x.Players)
                .ThenInclude(x => x.Player)
                .SingleOrDefault(x => x.TeamId == team.TeamId);

            if (teamFromContext == null)
            {
                return;
            }

            teamFromContext.Name = team.Name;

            // Also want to cross reference Players on the team
            foreach (var playerFromContext in teamFromContext.Players.ToList())
            {
                if (!team.Players.Any(x => x.PlayerId == playerFromContext.PlayerId))
                {
                    _logger.LogInformation("Deleting player: {0} from team: {1} with teamid: {2}.  As a cross reference the team id from client was {3}.",
                        playerFromContext.Player.Name,
                        teamFromContext.Name,
                        teamFromContext.TeamId,
                        team.TeamId
                        );

                    teamFromContext.Players.Remove(playerFromContext);
                    _context.TeamRosters.Remove(playerFromContext);
                }
            }

            foreach (var player in team.Players)
            {
                if (!teamFromContext.Players.Any(x => x.PlayerId == player.PlayerId))
                {
                    _logger.LogInformation("Adding player: {0} with playerid: {1}",
                        player.Player.Name,
                        player.PlayerId);

                    _context.TeamRosters.Add(
                        new TeamRoster
                        {
                            PlayerId = player.PlayerId,
                            TeamId = team.TeamId
                        }
                        );
                }
            }


            _context.SaveChanges();
        }

        public void AddSession(Session session, int managerId)
        {
            var manager = _context.Managers.Include(x => x.Sessions).Single(x => x.ManagerId == managerId);

            if (manager == null)
            {
                return;
            }

            manager.Sessions.Add(session);

            _context.SaveChanges();
        }

        public Session GetSession(int sessionId)
        {
            var session = _context.Sessions.SingleOrDefault(x => x.SessionId == sessionId);

            if (session == null)
            {
                return null;
            }

            return session;
        }

        public IEnumerable<Session> GetAllSessions(int managerId)
        {
            var sessions =
                _context.Sessions
                .Include(x => x.Teams)
                .ThenInclude(x => x.Players)
                .ThenInclude(x => x.Player)
                .Include(x => x.Schedules)
                .ThenInclude(x => x.Matches)
                .ThenInclude(x => x.AwayTeam)
                .Include(x => x.Schedules)
                .ThenInclude(x => x.Matches)
                .ThenInclude(x => x.HomeTeam)
                .Include(x => x.Schedules)
                .ThenInclude(x => x.Matches)
                .ThenInclude(x => x.Scorecards)
                .Where(x => x.ManagerId == managerId);

            return sessions.AsEnumerable();
        }

        public void UpdateSession(Session session)
        {
            var sessionFromContext = _context.Sessions.SingleOrDefault(x => x.SessionId == session.SessionId);

            if (sessionFromContext == null)
            {
                return;
            }

            sessionFromContext.Name = session.Name;

            _context.SaveChanges();
        }

        public Session RemoveSession(int sessionId)
        {
            var session = _context.Sessions.SingleOrDefault(x => x.SessionId == sessionId);

            _context.Sessions.Remove(session);

            _context.SaveChanges();

            return session;
        }

        public IEnumerable<Player> GetAllPlayersFromTeam(int teamId)
        {
            var team = _context.Teams.Include(x => x.Players).ThenInclude(z => z.Player).SingleOrDefault(y => y.TeamId == teamId);

            return team.Players.Select(x => x.Player).AsEnumerable();
        }

        public Player GetPlayerFromTeam(int playerId, int teamId)
        {
            var team = _context.Teams.Include(x => x.Players).SingleOrDefault(y => y.TeamId == teamId);

            if (team == null)
            {
                return null;
            }

            var teamRoster = team.Players.SingleOrDefault(x => x.PlayerId == playerId);

            return teamRoster.Player;
        }

        public void AddPlayerToTeam(Player player, int teamId)
        {
            var team = _context.Teams.Include(x => x.Players).SingleOrDefault(y => y.TeamId == teamId);

            if (team == null)
            {
                return;
            }

            var playerFromContext = _context.Players.SingleOrDefault(x => x.PlayerId == player.PlayerId);

            if (playerFromContext == null)
            {
                return;
            }

            var teamRoster =
                new TeamRoster
                {
                    PlayerId = playerFromContext.PlayerId,
                    TeamId = teamId
                };

            team.Players.Add(teamRoster);

            _context.SaveChanges();
        }

        public Player RemovePlayerFromTeam(int playerId, int teamId)
        {
            var team = _context.Teams.Include(x => x.Players).ThenInclude(z => z.Player).SingleOrDefault(y => y.TeamId == teamId);

            if (team == null)
            {
                return null;
            }

            var teamRoster = team.Players.SingleOrDefault(x => x.PlayerId == playerId);

            if (teamRoster == null)
            {
                return null;
            }

            _context.TeamRosters.Remove(teamRoster);

            _context.SaveChanges();

            return teamRoster.Player;
        }

        public IEnumerable<Schedule> GetAllSchedules()
        {
            return _context.Schedules.AsEnumerable();
        }

        public IEnumerable<Schedule> GetAllSchedulesFromSession(int sessionId)
        {
            var session =
                _context.Sessions
                .Include(x => x.Schedules)
                .ThenInclude(y => y.Matches)
                .ThenInclude(z => z.HomeTeam)
                .Include(x => x.Schedules)
                .ThenInclude(y => y.Matches)
                .ThenInclude(z => z.AwayTeam)
                .Include(x => x.Schedules)
                .ThenInclude(x => x.Matches)
                .ThenInclude(x => x.Scorecards)
                .SingleOrDefault(y => y.SessionId == sessionId);

            if (session == null)
            {
                return null;
            }

            return session.Schedules.AsEnumerable();
        }

        public Schedule GetScheduleById(int scheduleId)
        {
            return _context.Schedules.SingleOrDefault(x => x.ScheduleId == scheduleId);
        }

        public Schedule GetScheduleByIdFromSession(int scheduleId, int sessionId)
        {
            var session = _context.Sessions.Include(x => x.Schedules).SingleOrDefault(y => y.SessionId == sessionId);

            if (session == null)
            {
                return null;
            }

            return session.Schedules.SingleOrDefault(x => x.SessionId == sessionId);
        }

        public void AddScheduleToSession(Schedule schedule, int sessionId, IEnumerable<TeamMatch> matches)
        {
            var session = _context.Sessions.Include(x => x.Schedules).ThenInclude(y => y.Matches).ThenInclude(z => z.Scorecards).SingleOrDefault(y => y.SessionId == sessionId);

            if (session == null)
            {
                return;
            }

            session.Schedules.Add(schedule);

            _context.SaveChanges();

            // get the last added schedule
            var scheduleFromContext = _context.Schedules.Last();

            int count = 0;

            foreach (var match in matches)
            {
                var newMatch = new TeamMatch();
                newMatch.ScheduleId = scheduleFromContext.ScheduleId;
                newMatch.HomeTeamId = match.HomeTeam.TeamId;
                newMatch.AwayTeamId = match.AwayTeam.TeamId;
                _context.TeamMatches.Add(newMatch);

                _context.SaveChanges();

                var lastMatch = _context.TeamMatches.Last().TeamMatchId;

                // Add appropriate score cards to match
                if (session.Format == Format.EightBall)
                {
                    _context.Scorecards.Add(
                        new Scorecard
                        {
                            Format = Format.EightBall,
                            State = ScorecardState.Initial,
                            TeamMatchId = lastMatch
                        }
                        );
                }
                else if (session.Format == Format.NineBall)
                {
                    _context.Scorecards.Add(
                        new Scorecard
                        {
                            Format = Format.NineBall,
                            State = ScorecardState.Initial,
                            TeamMatchId = lastMatch
                        }
                        );
                }
                else if (session.Format == Format.EightBallNineBall)
                {
                    //if (count % 2 == 0)
                    {
                        _context.Scorecards.Add(
                        new Scorecard
                        {
                            Format = Format.EightBall,
                            State = ScorecardState.Initial,
                            TeamMatchId = lastMatch
                        }
                        );
                        _context.Scorecards.Add(
                        new Scorecard
                        {
                            Format = Format.NineBall,
                            State = ScorecardState.Initial,
                            TeamMatchId = lastMatch
                        }
                        );
                    }
                }
                else if (session.Format == Format.EightBall_Double)
                {
                    _context.Scorecards.Add(
                        new Scorecard
                        {
                            Format = Format.EightBall,
                            State = ScorecardState.Initial,
                            TeamMatchId = lastMatch
                        }
                        );
                    _context.Scorecards.Add(
                        new Scorecard
                        {
                            Format = Format.EightBall,
                            State = ScorecardState.Initial,
                            TeamMatchId = lastMatch
                        }
                        );
                }

                count++;
            }

            session.ScheduleCreated = true;

            _context.SaveChanges();
        }

        public void UpdateSchedule(Schedule schedule)
        {
            var scheduleFromContext = _context.Schedules.SingleOrDefault(x => x.ScheduleId == schedule.ScheduleId);

            if (scheduleFromContext == null)
            {
                return;
            }

            // Update the date or time
            scheduleFromContext.Date = scheduleFromContext.Date;
            scheduleFromContext.Time = schedule.Time;

            _context.SaveChanges();
        }

        public Schedule RemoveSchedule(int scheduleId)
        {
            var scheduleFromContext = _context.Schedules.SingleOrDefault(x => x.ScheduleId == scheduleId);

            if (scheduleFromContext == null)
            {
                return null;
            }

            _context.Schedules.Remove(scheduleFromContext);
            _context.SaveChanges();

            return scheduleFromContext;
        }

        public void AddVenue(Venue venue)
        {
            _context.Venues.Add(venue);
            _context.SaveChanges();
        }

        public Venue GetVenue(int venueId)
        {
            return _context.Venues.SingleOrDefault(x => x.VenueId == venueId);
        }

        public IEnumerable<Venue> GetAllVenues()
        {
            return _context.Venues.AsEnumerable();
        }

        public Venue RemoveVenue(int venueId)
        {
            var venueFromContext = _context.Venues.SingleOrDefault(x => x.VenueId == venueId);

            if (venueFromContext == null)
            {
                return null;
            }

            _context.Venues.Remove(venueFromContext);
            _context.SaveChanges();

            return venueFromContext;
        }

        public object GetTeamById(int teamId)
        {
            var teamFromContext = _context.Teams.SingleOrDefault(x => x.TeamId == teamId);

            if (teamFromContext == null)
            {
                return null;
            }

            return teamFromContext;
        }

        public Scorecard GetScorecardById(int scorecardId)
        {
            var scorecardFromContext = _context.Scorecards.SingleOrDefault(x => x.ScorecardId == scorecardId);

            if (scorecardFromContext == null)
            {
                return null;
            }

            return scorecardFromContext;
        }

        public void CreateMatchesForScorecard(ScorecardViewModel viewModel)
        {
            // Keep track of unique PlayerIds from lineup (to create one PlayerResult)
            HashSet<int> uniquePlayers = new HashSet<int>();

            var scorecardFromContext = _context.Scorecards.SingleOrDefault(x => x.ScorecardId == viewModel.ScorecardId);

            if (scorecardFromContext == null)
            {
                return;
            }

            int homePlayerCount = viewModel.HomePlayers.Count;
            int awayPlayerCount = viewModel.AwayPlayers.Count;

            // Calculate the number of total matches
            int rounds = 0;

            switch (viewModel.MatchupType)
            {
                case MatchupType.FiveOnFive:
                    rounds = 5;
                    break;
                case MatchupType.FiveOnFour:
                case MatchupType.FourOnFour:
                    rounds = 4;
                    break;
                case MatchupType.ThreeOnThree:
                    rounds = 3;
                    break;
            }

            int matchCount = homePlayerCount * rounds;

            int homePlayerIndex = 0;
            int awayPlayerIndex = 0;
            int currentRound = 1;

            for (int i = 1; i <= matchCount; i++)
            {
                var homePlayer = viewModel.HomePlayers.ElementAt(homePlayerIndex);
                var awayPlayer = viewModel.AwayPlayers.ElementAt(awayPlayerIndex);
                int homePlayerId = homePlayer.PlayerId;
                int awayPlayerId = awayPlayer.PlayerId;

                var match = new PlayerMatch();

                match.ScorecardId = viewModel.ScorecardId;
                match.HomePlayerId = homePlayerId;
                match.AwayPlayerId = awayPlayerId;

                match.HomePlayerScore = new PlayerScore();
                match.AwayPlayerScore = new PlayerScore();

                if (!uniquePlayers.Contains(homePlayer.PlayerId))
                {
                    uniquePlayers.Add(homePlayer.PlayerId);

                    var playerResult =
                        new PlayerResult
                        {
                            PlayerId = homePlayerId,
                            ScorecardId = viewModel.ScorecardId,
                            Handicap = homePlayer.Handicap,
                            Format = viewModel.Format,
                            PlayCount = 1
                        };

                    _context.PlayerResults.Add(playerResult);
                }
                else
                {
                    if (currentRound == 1)
                    {
                        var player =
                            _context.PlayerResults.Single(r => r.PlayerId == homePlayer.PlayerId && r.ScorecardId == viewModel.ScorecardId);

                        player.PlayCount++;
                    }
                }

                if (!uniquePlayers.Contains(awayPlayer.PlayerId))
                {
                    uniquePlayers.Add(awayPlayer.PlayerId);

                    var playerResult =
                        new PlayerResult
                        {
                            PlayerId = awayPlayerId,
                            ScorecardId = viewModel.ScorecardId,
                            Handicap = awayPlayer.Handicap,
                            Format = viewModel.Format,
                            PlayCount = 1
                        };

                    _context.PlayerResults.Add(playerResult);
                }
                else
                {
                    if (currentRound == 1)
                    {
                        var player =
                            _context.PlayerResults.Single(r => r.PlayerId == awayPlayer.PlayerId && r.ScorecardId == viewModel.ScorecardId);

                        player.PlayCount++;
                    }
                }

                _context.PlayerScores.Add(match.HomePlayerScore);
                _context.PlayerScores.Add(match.AwayPlayerScore);
                _context.PlayerMatches.Add(match);
                _context.SaveChanges();

                homePlayerIndex++;
                awayPlayerIndex++;

                if (i % homePlayerCount == 0)
                {
                    currentRound++;
                    homePlayerIndex = 0;
                    awayPlayerIndex = currentRound - 1;
                }

                if (awayPlayerIndex > awayPlayerCount - 1)
                {
                    awayPlayerIndex = 0;
                }
            }

            scorecardFromContext.State = ScorecardState.InProgress;
            scorecardFromContext.NumberOfTables = viewModel.NumberOfTables;
            _context.SaveChanges();
        }

        public IEnumerable<TeamRoster> GetAllPlayersFromSession(int sessionId)
        {
            var playersInSession =
                _context.TeamRosters
                .Include(x => x.Team)
                .Include(x => x.Player)
                .Where(x => x.Team.SessionId == sessionId)
                .OrderBy(x => x.Player.Name)
                .ToList()
                .GroupBy(x => x.PlayerId)
                .Select(x => x.First());

            return playersInSession;
        }

        public void UpdateHandicapForPlayer(int playerId, int sessionId, int handicap)
        {
            var players =
                _context.TeamRosters
                .Include(x => x.Team)
                .Where(
                    x =>
                    x.PlayerId == playerId &&
                    x.Team.SessionId == sessionId);

            if (players == null)
            {
                return;
            }

            foreach (var player in players)
            {
                player.Handicap = handicap;
            }

            _context.SaveChanges();
        }

        public void UpdatePlayerScores(PlayerMatchViewModel viewModel, out int homePlayerScore, out int awayPlayerScore, out int scorecardState)
        {
            homePlayerScore = 0;
            awayPlayerScore = 0;
            scorecardState = 0;

            var playerMatchFromContext =
                _context.PlayerMatches
                .Include(x => x.HomePlayerScore)
                .Include(x => x.AwayPlayerScore)
                .Include(x=>x.Scorecard)
                .SingleOrDefault(x => x.PlayerMatchId == viewModel.PlayerMatchId);

            if (playerMatchFromContext == null)
            {
                return;
            }

            playerMatchFromContext.HomePlayerScore.Score = viewModel.HomePlayerScore;
            playerMatchFromContext.AwayPlayerScore.Score = viewModel.AwayPlayerScore;

            if (viewModel.HomePlayerScore > 0 ||
                viewModel.AwayPlayerScore > 0)
            {
                playerMatchFromContext.Saved = true;
            }

            _context.SaveChanges();

            playerMatchFromContext =
                _context.PlayerMatches
                .Include(x => x.HomePlayerScore)
                .Include(x => x.AwayPlayerScore)
                .SingleOrDefault(x => x.PlayerMatchId == viewModel.PlayerMatchId);

            homePlayerScore = playerMatchFromContext.HomePlayerScore.Score;
            awayPlayerScore = playerMatchFromContext.AwayPlayerScore.Score;
            scorecardState = (int)playerMatchFromContext.Scorecard.State;
        }
    }
}
