namespace Mippa.ViewModels
{
    public class PlayerMatchViewModel
    {
        public int PlayerMatchId { get; set; }
        public PlayerViewModel HomePlayer { get; set; }
        public int HomePlayerScore { get; set; }
        public PlayerViewModel AwayPlayer { get; set; }
        public int AwayPlayerScore { get; set; }
        public bool HomeBreaking { get; set; }
        public bool Saved { get; set; }
    }
}