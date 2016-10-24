using System.ComponentModel.DataAnnotations;

namespace Mippa.Models
{
    public class ResetRequest
    {
        [Key]
        public int ResetRequestId { get; set; }
        public int ScorecardId { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public ResetPreference Preference { get; set; }

        public virtual Scorecard Scorecard { get; set; }
    }
}
