using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.Models
{
    public class Scorecard
    {
        public int ScorecardId { get; set; }
        public int TeamMatchId { get; set; }
        public ScorecardState State { get; set; }
        public Format Format { get; set; }

        [JsonIgnore]
        public virtual TeamMatch TeamMatch { get; set; }

        [JsonIgnore]
        public virtual ICollection<PlayerMatch> PlayerMatches { get; set; }
        [JsonIgnore]
        public virtual ICollection<PlayerResult> PlayerResults { get; set; }
        [JsonIgnore]
        public virtual ICollection<TeamResult> TeamResults { get; set; }
    }
}
