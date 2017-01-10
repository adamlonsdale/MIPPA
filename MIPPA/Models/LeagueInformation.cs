using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIPPA.Models
{
    public class LeagueInformation
    {
        public int LeagueInformationId { get; set; }
        public int PlayerCount { get; set; }
        public string PlayerFund { get; set; }
        public string AdminFund { get; set; }
    }
}
