﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIPPA.ViewModels.Scheduler
{
    public class WeekViewModel
    {
        public string Date { get; set; }
        public  string Time { get; set; }
        public bool PreviousWeekExists { get; set; }
        public bool NextWeekExists { get; set; }

        // Collection of Matches
        public IEnumerable<MatchViewModel> MatchViewModels { get; set; }

        // Collection of Teams
        public IEnumerable<TeamViewModel> TeamViewModels { get; set; }
    }
}
