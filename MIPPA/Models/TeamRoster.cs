using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mippa.Models
{
    public class TeamRoster
    {
        [Key]
        public int TeamRosterId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }

        [JsonIgnore]
        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; }

        [ForeignKey("PlayerId")]
        public virtual Player Player { get; set; }

        public int Handicap { get; set; }
    }
}
