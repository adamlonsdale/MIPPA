using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.ViewModels.Statistics
{
    public class PlayerStatisticsViewModel
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int TotalScore { get; set; }
        public int TotalWins { get; set; }
        public int PlayCount { get; set; }
        public double ProjectedHandicap { get; set; }
    }
}
