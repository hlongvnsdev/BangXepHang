using System.ComponentModel.DataAnnotations;

namespace BangXepHang.Models
{
    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Avatar { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public virtual ICollection<GameScore> GameScores { get; set; } = new List<GameScore>();
    }
}
