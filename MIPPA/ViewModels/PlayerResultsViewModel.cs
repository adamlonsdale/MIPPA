using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.ViewModels
{
    public class PlayerResultsViewModel
    {
        public ICollection<PlayerViewModel> HomePlayerResults { get; set; }
        public ICollection<PlayerViewModel> AwayPlayerResults { get; set; }

        public PlayerResultsViewModel()
        {
            HomePlayerResults = new List<PlayerViewModel>();
            AwayPlayerResults = new List<PlayerViewModel>();
        }
    }
}
