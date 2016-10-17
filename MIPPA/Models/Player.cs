using System.ComponentModel.DataAnnotations;

namespace Mippa.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public string Name { get; set; }
    }
}
