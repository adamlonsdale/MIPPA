using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mippa.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public int SessionId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int Index { get; set; }

        [JsonIgnore]
        public virtual Session Session { get; set; }
        public virtual ICollection<TeamMatch> Matches { get; set; }
        
    }
}
