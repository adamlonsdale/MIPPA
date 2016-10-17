using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mippa.Models
{
    public class Scheduler
    {
        // Need the SessionId (Manager and League is inferred from this)
        public int SessionId;
        // Need the start date
        public string Date;
        // Need the time
        public string Time;
        // Need the number of teams
        public int NumberOfPlays;

        // Need an enumerable of Teams in the index order set
        public IEnumerable<Team> teams { get; set; }
    }
}
