using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIPPA.ViewModels
{
    public class HandicapsViewModel
    {
        // Handicap information sorted by player name
        IEnumerable<HandicapViewModel> Handicaps { get; set; }
    }
}
