using Mippa.Models;
using System.Collections.Generic;

namespace Mippa.ViewModels
{
    public class ScorecardViewModel
    {
        public int ScorecardId { get; set; }
        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public ICollection<PlayerViewModel> HomePlayers { get; set; }
        public ICollection<PlayerViewModel> AwayPlayers { get; set; }
        public ScorecardState State { get; set; }
        public Format Format { get; set; }
        public ICollection<RoundViewModel> Rounds { get; set; }
        public MatchupType MatchupType { get; set; }
        public int OtherScorecardId { get; set; }
        public int NumberOfTables { get; set; }
    }
}
