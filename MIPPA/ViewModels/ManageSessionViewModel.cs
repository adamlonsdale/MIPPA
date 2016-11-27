using Mippa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIPPA.ViewModels
{
    public class ManageSessionViewModel
    {
        public int SessionId { get; set; }
        public int ManagerId { get; set; }
        public string Name { get; set; }
        public Format Format { get; set; }
        public MatchupType MatchupType { get; set; }
        public bool ScheduleCreated { get; set; }
        public IEnumerable<TeamViewModel> Teams { get; set; }
    }
}
