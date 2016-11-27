using Mippa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MIPPA.ViewModels;

namespace Mippa.Models
{
    /// <summary>
    /// The Repository interface provides the public facing methods that access and manipulate the underlying data
    /// 
    /// -- Managers
    /// 
    /// -- Add manager
    /// -- Update manager
    /// -- Get Managers
    /// </summary>
    public interface IRepository
    {
        void FinalizeMatch(int scorecardId);
        LoginViewModel GetLoginViewModel(LoginViewModel viewModel);
        void FinalizeAllScorecards();
        StatisticsViewModel GetStatisticsForSession(int sessionId, Format format = Format.EightBall);
        PlayerResultsViewModel CalculatePlayerResults(int scorecardId, bool saveResults = false);
        void ResetScorecard(int scorecardId);
        TeamResultsViewModel CalculateTeamResults(int scorecardId, bool saveResults);
        void UpdatePlayerScores(PlayerMatchViewModel viewModel, out int homePlayerScore, out int awayPlayerScore, out int scorecardState);
        IEnumerable<HandicapViewModel> GetHandicapViewModelsForSession(int sessionId);
        IEnumerable<ResetRequest> GetResetRequestsForSession(int sessionId);
        void RequestReset(int scorecardId, ResetRequest request);
        ScorecardViewModel GetScorecardInformation(int scorecardId);
        ScorecardViewModel GetScorecardInformation_Initial(int scorecardId);
        ScorecardViewModel GetScorecardInformation_NotInitial(int scorecardId);

        void CreateMatchesForScorecard(ScorecardViewModel viewModel);
        void UpdateHandicapForPlayer(int playerId, int sessionId, int handicap);

        #region Managers

        void AddManager(Manager manager);
        Manager GetManager(int id);
        IEnumerable<Manager> GetAllManagers();
        Scorecard GetScorecardById(int scorecardId);
        void UpdateManager(Manager manager);
        Manager RemoveManager(int id);
        IEnumerable<TeamRoster> GetAllPlayersFromSession(int sessionId);

        #endregion

        #region Sessions

        void AddSession(Session session, int managerId);
        Session GetSession(int sessionId);
        IEnumerable<ManageSessionViewModel> GetAllSessions(int managerId);
        void UpdateSession(Session session);
        Session RemoveSession(int id);

        #endregion

        #region Players

        void AddPlayer(Player player);
        Player GetPlayer(int playerId);
        IEnumerable<Player> GetAllPlayers();
        IEnumerable<PlayerQueryViewModel> GetPlayersFromQuery(string aQuery, int? sessionId = null);
        void UpdatePlayer(Player player);
        Player RemovePlayer(int playerId);

        #endregion

        #region Teams

        void AddTeamToSession(Team team, int sessionId);
        IEnumerable<Team> GetAllTeams();
        Team GetTeamFromSessionByName(string name, int sessionId);
        IEnumerable<Team> GetAllTeamsFromSession(int sessionId);
        void UpdateTeam(Team team);
        Team RemoveTeam(int id);
        #endregion

        #region TeamRosters

        IEnumerable<Player> GetAllPlayersFromTeam(int teamId);

        Player GetPlayerFromTeam(int playerId, int teamId);

        void AddPlayerToTeam(Player player, int teamId);

        Player RemovePlayerFromTeam(int playerId, int teamId);

        #endregion

        #region Schedules

        IEnumerable<Schedule> GetAllSchedules();

        IEnumerable<Schedule> GetAllSchedulesFromSession(int sessionId);

        Schedule GetScheduleById(int scheduleId);

        Schedule GetScheduleByIdFromSession(int scheduleId, int sessionId);

        void AddScheduleToSession(Schedule schedule, int sessionId, IEnumerable<TeamMatch> matches);

        void UpdateSchedule(Schedule schedule);

        Schedule RemoveSchedule(int scheduleId);

        #endregion

        #region Venues

        void AddVenue(Venue venue);
        Venue GetVenue(int venueId);
        IEnumerable<Venue> GetAllVenues();
        Venue RemoveVenue(int venueId);


        #endregion
        
        object GetTeamById(int teamId);
    }
}
