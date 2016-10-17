namespace Mippa.Models
{
    public class TeamResult
    {
        public int TeamResultId { get; set; }
        public int ScorecardId { get; set; }
        public int TeamId { get; set; }
        public int Score { get; set; }
        public double Wins { get; set; }
        public int Dues { get; set; }

        public virtual Scorecard Scorecard { get; set; }
        public virtual Team Team { get; set; }
    }
}