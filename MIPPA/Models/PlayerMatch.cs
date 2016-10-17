namespace Mippa.Models
{
    public class PlayerMatch
    {
        public int PlayerMatchId { get; set; }
        public int ScorecardId { get; set; }
        public int HomePlayerId { get; set; }
        public int HomePlayerScoreId { get; set; }
        public int AwayPlayerId { get; set; }
        public int AwayPlayerScoreId { get; set; }

        public virtual Scorecard Scorecard { get; set; }
        public virtual Player HomePlayer { get; set; }
        public virtual Player AwayPlayer { get; set; }
        public virtual PlayerScore HomePlayerScore { get; set; }
        public virtual PlayerScore AwayPlayerScore { get; set; }
        public bool Saved { get; set; }

    }
}