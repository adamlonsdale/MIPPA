using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mippa.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        public int SessionId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string UserName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public int Index;
        public bool Bye { get; set; }

        [JsonIgnore]
        public virtual Session Session { get; set; }
        public virtual ICollection<TeamRoster> Players { get; set; }
    }
}
