using Mippa.Models;
using Mippa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIPPA.ViewModels
{
    public class TeamViewModel
    {
        public string Name { get; set; }
        public int TeamId { get; set; }
        public IEnumerable<PlayerViewModel> Players { get; set; }
        public bool Scheduled { get; set; }
        public int Index { get; set; }
    }
}
