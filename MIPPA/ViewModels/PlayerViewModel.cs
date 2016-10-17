using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.ViewModels
{
    public class PlayerViewModel
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int TotalScore { get; set; }
        public int Handicap { get; set; }
    }
}
