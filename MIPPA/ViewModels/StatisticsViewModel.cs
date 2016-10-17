using Mippa.ViewModels.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.ViewModels
{
    public class StatisticsViewModel
    {
        // We need all the teams
        public ICollection<TeamStatisticsViewModel> TeamStatistics { get; set; }
        public ICollection<PlayerStatisticsViewModel> PlayerStatistics { get; set; }

        public StatisticsViewModel()
        {
            TeamStatistics = new List<TeamStatisticsViewModel>();
            PlayerStatistics = new List<PlayerStatisticsViewModel>();
        }
    }
}
