using Mippa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.Utilities
{
    public static class Common
    {
        public static IRepository _repository { get; set; }

        public static void ScheduleSession(IRepository repo, Scheduler scheduler, int pivotTeam = 1)
        {
            _repository = repo;

            List<Team> teams = new List<Team>();
            List<Team> firstHalf = new List<Team>();
            List<Team> secondHalf = new List<Team>();

            int numberOfTeams = scheduler.teams.Count();
            int numberOfPlays = scheduler.NumberOfPlays;

            string date = scheduler.Date;
            DateTime lastKnownDate = DateTime.Parse(date);

            teams.AddRange(scheduler.teams.ToList());

            // Split the teams in half
            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].Index = i + 1;
                if (i <= teams.Count / 2 - 1)
                {
                    firstHalf.Add(teams[i]);
                }
                else
                {
                    secondHalf.Add(teams[i]);
                }
            }

            // Reverse the second half
            secondHalf.Reverse();

            int matchupsPerWeek = teams.Count / 2;
            int numberOfWeeks = (numberOfTeams - 1) * numberOfPlays;

            for (int i = 1; i <= numberOfWeeks; i++)
            {
                // Once we know home and away team we will create a new schedule

                Schedule newSchedule = new Schedule();

                newSchedule.Date = date;
                newSchedule.Time = scheduler.Time;
                newSchedule.SessionId = scheduler.SessionId;

                Console.WriteLine("Week " + i);

                HashSet<TeamMatch> matches = new HashSet<TeamMatch>();

                for (int j = 0; j < matchupsPerWeek; j++)
                {
                    Team awayTeam;
                    Team homeTeam;

                    if ((firstHalf[j].Index + secondHalf[j].Index) % 2 == 1)
                    {
                        if (firstHalf[j].Index < secondHalf[j].Index)
                        {
                            homeTeam = firstHalf[j];
                            awayTeam = secondHalf[j];
                        }
                        else
                        {
                            homeTeam = secondHalf[j];
                            awayTeam = firstHalf[j];
                        }
                    }
                    else
                    {
                        if (firstHalf[j].Index < secondHalf[j].Index)
                        {
                            awayTeam = firstHalf[j];
                            homeTeam = secondHalf[j];
                        }
                        else
                        {
                            awayTeam = secondHalf[j];
                            homeTeam = firstHalf[j];
                        }
                    }

                    // Temporary Fix


                    matches.Add(
                        new TeamMatch
                        {
                            AwayTeam = awayTeam,
                            HomeTeam = homeTeam
                        }
                        );
                }

                _repository.AddScheduleToSession(newSchedule, scheduler.SessionId, matches);

                lastKnownDate = lastKnownDate.AddDays(7);
                date = lastKnownDate.ToString("d");

                RoundRobin(firstHalf, secondHalf, teams[pivotTeam -1]);
            }
        }

        private static void RoundRobin(List<Team> firstHalf, List<Team> secondHalf, Team pivotTeam = null)
        {
            // Combine the two into 1 array
            List<Team> allTeams = new List<Team>();
            allTeams.AddRange(secondHalf.ToList());
            allTeams.Reverse();
            allTeams.AddRange(firstHalf.ToList());

            // Get the index of the pivot team
            int pivotTeamIndex = allTeams.IndexOf(pivotTeam);

            if (pivotTeamIndex == 0)
            {
                // Pivot is zero
                // Move last to just after pivot
                allTeams.Insert(1, allTeams.Last());
                allTeams.RemoveAt(allTeams.Count - 1);
            }
            else if (pivotTeamIndex == allTeams.Count - 1)
            {
                // Pivot is last
                // Remove previous
                // Insert previous to 0 position
                Team previous = allTeams[pivotTeamIndex - 1];
                allTeams.RemoveAt(pivotTeamIndex - 1);
                allTeams.Insert(0, previous);
            }
            else
            {
                // Somewhere in the middle
                // Remove previous
                // Make previous next
                // Remove last
                // Make last first
                Team previous = allTeams[pivotTeamIndex - 1];

                allTeams.Insert(pivotTeamIndex + 1, previous);
                allTeams.RemoveAt(pivotTeamIndex - 1);
                allTeams.Insert(0, allTeams.Last());
                allTeams.RemoveAt(allTeams.Count - 1);

            }

            firstHalf.Clear();
            secondHalf.Clear();

            // Reset the first and second half
            // Split the teams in half
            for (int i = 0; i < allTeams.Count; i++)
            {
                if (i <= allTeams.Count / 2 - 1)
                {
                    secondHalf.Add(allTeams[i]);
                }
                else
                {
                    firstHalf.Add(allTeams[i]);
                }
            }

            // Reverse the second half
            secondHalf.Reverse();

        }
    }
}
