namespace Mippa.Models
{
    public class PlayerResult
    {
        public int PlayerResultId { get; set; }
        public int ScorecardId { get; set; }
        public int PlayerId { get; set; }
        public Format Format { get; set; }
        public int Score { get; set; }
        public int AdjustedScore { get; set; }
        public int Handicap { get; set; }
        public int Wins { get; set; }
        public int Dues { get; set; }
        public int PlayCount { get; set; }

        public virtual Scorecard Scorecard { get; set; }
        public virtual Player Player { get; set; }
    }
}