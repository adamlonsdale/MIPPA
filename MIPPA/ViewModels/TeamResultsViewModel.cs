using Mippa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.ViewModels
{
    public class TeamResultsViewModel
    {
        public int HomeTeamTotalPoints { get; set; }
        public double HomeTeamTotalRounds { get; set; }
        public int HomeTeamHandicap { get; set; }
        public int AwayTeamTotalPoints { get; set; }
        public double AwayTeamTotalRounds { get; set; }
        public int AwayTeamHandicap { get; set; }
        public IList<double> HomeRoundsWon { get; set; }
        public IList<double> AwayRoundsWon { get; set; }
        public ScorecardState ScorecardState { get; set; }
    }
}
