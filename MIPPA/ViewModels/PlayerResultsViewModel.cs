using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.ViewModels
{
    public class PlayerResultsViewModel
    {
        public ICollection<PlayerViewModel> Players { get; set; }

        public PlayerResultsViewModel()
        {
            Players = new HashSet<PlayerViewModel>();
        }
    }
}
