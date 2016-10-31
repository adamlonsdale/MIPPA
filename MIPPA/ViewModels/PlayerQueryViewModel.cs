using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.ViewModels
{
    public class PlayerQueryViewModel
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int Handicap { get; set; }
        public bool ExistsInSession { get; set; }
    }
}
