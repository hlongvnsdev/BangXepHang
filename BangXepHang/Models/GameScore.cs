using System.ComponentModel.DataAnnotations;

namespace BangXepHang.Models
{
    public class GameScore
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }
        
        [Required]
        public DateTime PlayTime { get; set; }
        
        [Required]
        public int Score { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public virtual Customer Customer { get; set; } = null!;
    }
}
