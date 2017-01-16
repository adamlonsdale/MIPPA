using MIPPA.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mippa.Models
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }
        public int ManagerId { get; set; }
        public string Name { get; set; }
        public Format Format { get; set; }
        public MatchupType MatchupType { get; set; }
        public bool ScheduleCreated { get; set; }

        [JsonIgnore]
        public virtual Manager Manager { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        [NotMapped]
        public ScheduleViewModel ScheduleViewModel { get; set; }

        public bool Active { get; set; }
    }
}
