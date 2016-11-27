using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mippa.Models
{
    public class TeamMatch
    {
        [Key]
        public int TeamMatchId { get; set; }
        public int ScheduleId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public string Location { get; set; }

        [JsonIgnore]
        public virtual Schedule Schedule { get; set; }
        [ForeignKey("HomeTeamId")]
        public virtual Team HomeTeam { get; set; }
        [ForeignKey("AwayTeamId")]
        public virtual Team AwayTeam { get; set; }
        
        public virtual ICollection<Scorecard> Scorecards { get; set; }
    }
}
