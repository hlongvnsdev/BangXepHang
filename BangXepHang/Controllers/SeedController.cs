using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BangXepHang.Data;
using BangXepHang.Models;

namespace BangXepHang.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeedController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Khởi tạo dữ liệu mẫu
        /// </summary>
        /// <returns>Kết quả khởi tạo</returns>
        [HttpPost("init")]
        public async Task<ActionResult<string>> InitializeSampleData()
        {
            try
            {
                // Xóa dữ liệu cũ nếu có
                _context.GameScores.RemoveRange(_context.GameScores);
                _context.Customers.RemoveRange(_context.Customers);
                await _context.SaveChangesAsync();

                // Tạo khách hàng mẫu
                var customers = new List<Customer>
                {
                    new Customer { Name = "Nguyễn Văn An", Avatar = "https://example.com/avatar1.jpg" },
                    new Customer { Name = "Trần Thị Bình", Avatar = "https://example.com/avatar2.jpg" },
                    new Customer { Name = "Lê Văn Cường", Avatar = "https://example.com/avatar3.jpg" },
                    new Customer { Name = "Phạm Thị Dung", Avatar = "https://example.com/avatar4.jpg" },
                    new Customer { Name = "Hoàng Văn Em", Avatar = "https://example.com/avatar5.jpg" },
                    new Customer { Name = "Vũ Thị Phương", Avatar = "https://example.com/avatar6.jpg" },
                    new Customer { Name = "Đặng Văn Giang", Avatar = "https://example.com/avatar7.jpg" },
                    new Customer { Name = "Bùi Thị Hoa", Avatar = "https://example.com/avatar8.jpg" },
                    new Customer { Name = "Phan Văn Inh", Avatar = "https://example.com/avatar9.jpg" },
                    new Customer { Name = "Võ Thị Khoa", Avatar = "https://example.com/avatar10.jpg" }
                };

                _context.Customers.AddRange(customers);
                await _context.SaveChangesAsync();

                // Tạo điểm số mẫu cho tháng 1/2024
                var gameScores = new List<GameScore>
                {
                    // Nguyễn Văn An (ID: 1)
                    new GameScore { CustomerId = 1, PlayTime = new DateTime(2024, 1, 5, 10, 30, 0), Score = 1200 },
                    new GameScore { CustomerId = 1, PlayTime = new DateTime(2024, 1, 10, 14, 20, 0), Score = 1500 },
                    new GameScore { CustomerId = 1, PlayTime = new DateTime(2024, 1, 15, 9, 15, 0), Score = 1800 },
                    new GameScore { CustomerId = 1, PlayTime = new DateTime(2024, 1, 20, 16, 45, 0), Score = 2100 },
                    new GameScore { CustomerId = 1, PlayTime = new DateTime(2024, 1, 25, 11, 30, 0), Score = 1900 },

                    // Trần Thị Bình (ID: 2)
                    new GameScore { CustomerId = 2, PlayTime = new DateTime(2024, 1, 3, 8, 45, 0), Score = 1000 },
                    new GameScore { CustomerId = 2, PlayTime = new DateTime(2024, 1, 8, 13, 20, 0), Score = 1300 },
                    new GameScore { CustomerId = 2, PlayTime = new DateTime(2024, 1, 12, 15, 10, 0), Score = 1600 },
                    new GameScore { CustomerId = 2, PlayTime = new DateTime(2024, 1, 18, 10, 25, 0), Score = 1400 },
                    new GameScore { CustomerId = 2, PlayTime = new DateTime(2024, 1, 22, 17, 30, 0), Score = 1700 },

                    // Lê Văn Cường (ID: 3)
                    new GameScore { CustomerId = 3, PlayTime = new DateTime(2024, 1, 2, 12, 0, 0), Score = 1100 },
                    new GameScore { CustomerId = 3, PlayTime = new DateTime(2024, 1, 7, 14, 30, 0), Score = 1400 },
                    new GameScore { CustomerId = 3, PlayTime = new DateTime(2024, 1, 14, 9, 45, 0), Score = 1700 },
                    new GameScore { CustomerId = 3, PlayTime = new DateTime(2024, 1, 19, 16, 15, 0), Score = 2000 },
                    new GameScore { CustomerId = 3, PlayTime = new DateTime(2024, 1, 24, 11, 20, 0), Score = 1800 },

                    // Phạm Thị Dung (ID: 4)
                    new GameScore { CustomerId = 4, PlayTime = new DateTime(2024, 1, 4, 9, 30, 0), Score = 900 },
                    new GameScore { CustomerId = 4, PlayTime = new DateTime(2024, 1, 9, 15, 45, 0), Score = 1200 },
                    new GameScore { CustomerId = 4, PlayTime = new DateTime(2024, 1, 16, 8, 20, 0), Score = 1500 },
                    new GameScore { CustomerId = 4, PlayTime = new DateTime(2024, 1, 21, 13, 10, 0), Score = 1300 },
                    new GameScore { CustomerId = 4, PlayTime = new DateTime(2024, 1, 26, 16, 40, 0), Score = 1600 },

                    // Hoàng Văn Em (ID: 5)
                    new GameScore { CustomerId = 5, PlayTime = new DateTime(2024, 1, 1, 11, 15, 0), Score = 800 },
                    new GameScore { CustomerId = 5, PlayTime = new DateTime(2024, 1, 6, 14, 50, 0), Score = 1100 },
                    new GameScore { CustomerId = 5, PlayTime = new DateTime(2024, 1, 11, 10, 35, 0), Score = 1300 },
                    new GameScore { CustomerId = 5, PlayTime = new DateTime(2024, 1, 17, 15, 25, 0), Score = 1200 },
                    new GameScore { CustomerId = 5, PlayTime = new DateTime(2024, 1, 23, 12, 40, 0), Score = 1400 },

                    // Vũ Thị Phương (ID: 6)
                    new GameScore { CustomerId = 6, PlayTime = new DateTime(2024, 1, 13, 8, 10, 0), Score = 1000 },
                    new GameScore { CustomerId = 6, PlayTime = new DateTime(2024, 1, 28, 14, 20, 0), Score = 1300 },

                    // Đặng Văn Giang (ID: 7)
                    new GameScore { CustomerId = 7, PlayTime = new DateTime(2024, 1, 6, 16, 30, 0), Score = 1200 },
                    new GameScore { CustomerId = 7, PlayTime = new DateTime(2024, 1, 13, 11, 45, 0), Score = 1500 },
                    new GameScore { CustomerId = 7, PlayTime = new DateTime(2024, 1, 20, 9, 20, 0), Score = 1800 },
                    new GameScore { CustomerId = 7, PlayTime = new DateTime(2024, 1, 27, 15, 10, 0), Score = 1600 },

                    // Bùi Thị Hoa (ID: 8)
                    new GameScore { CustomerId = 8, PlayTime = new DateTime(2024, 1, 11, 12, 15, 0), Score = 1100 },
                    new GameScore { CustomerId = 8, PlayTime = new DateTime(2024, 1, 18, 16, 30, 0), Score = 1400 },
                    new GameScore { CustomerId = 8, PlayTime = new DateTime(2024, 1, 25, 10, 45, 0), Score = 1700 },

                    // Phan Văn Inh (ID: 9)
                    new GameScore { CustomerId = 9, PlayTime = new DateTime(2024, 1, 8, 9, 40, 0), Score = 900 },
                    new GameScore { CustomerId = 9, PlayTime = new DateTime(2024, 1, 15, 13, 25, 0), Score = 1200 },
                    new GameScore { CustomerId = 9, PlayTime = new DateTime(2024, 1, 22, 11, 50, 0), Score = 1500 },
                    new GameScore { CustomerId = 9, PlayTime = new DateTime(2024, 1, 29, 14, 35, 0), Score = 1300 },

                    // Võ Thị Khoa (ID: 10)
                    new GameScore { CustomerId = 10, PlayTime = new DateTime(2024, 1, 12, 10, 20, 0), Score = 1000 },
                    new GameScore { CustomerId = 10, PlayTime = new DateTime(2024, 1, 19, 15, 15, 0), Score = 1300 },
                    new GameScore { CustomerId = 10, PlayTime = new DateTime(2024, 1, 26, 8, 30, 0), Score = 1600 }
                };

                _context.GameScores.AddRange(gameScores);
                await _context.SaveChangesAsync();

                // Thêm dữ liệu cho tháng 2/2024
                var februaryScores = new List<GameScore>
                {
                    new GameScore { CustomerId = 1, PlayTime = new DateTime(2024, 2, 5, 10, 30, 0), Score = 2000 },
                    new GameScore { CustomerId = 1, PlayTime = new DateTime(2024, 2, 10, 14, 20, 0), Score = 2200 },
                    new GameScore { CustomerId = 2, PlayTime = new DateTime(2024, 2, 8, 13, 20, 0), Score = 1800 },
                    new GameScore { CustomerId = 2, PlayTime = new DateTime(2024, 2, 15, 15, 10, 0), Score = 2000 },
                    new GameScore { CustomerId = 3, PlayTime = new DateTime(2024, 2, 12, 14, 30, 0), Score = 1900 },
                    new GameScore { CustomerId = 3, PlayTime = new DateTime(2024, 2, 18, 9, 45, 0), Score = 2100 },
                    new GameScore { CustomerId = 4, PlayTime = new DateTime(2024, 2, 14, 15, 45, 0), Score = 1700 },
                    new GameScore { CustomerId = 5, PlayTime = new DateTime(2024, 2, 20, 11, 15, 0), Score = 1600 },
                    new GameScore { CustomerId = 6, PlayTime = new DateTime(2024, 2, 25, 8, 10, 0), Score = 1500 },
                    new GameScore { CustomerId = 7, PlayTime = new DateTime(2024, 2, 22, 16, 30, 0), Score = 1900 }
                };

                _context.GameScores.AddRange(februaryScores);
                await _context.SaveChangesAsync();

                return Ok($"Đã khởi tạo thành công dữ liệu mẫu:\n- {customers.Count} khách hàng\n- {gameScores.Count + februaryScores.Count} điểm số\n- Dữ liệu cho tháng 1/2024 và 2/2024");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi khởi tạo dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa tất cả dữ liệu
        /// </summary>
        /// <returns>Kết quả xóa</returns>
        [HttpDelete("clear")]
        public async Task<ActionResult<string>> ClearAllData()
        {
            try
            {
                _context.GameScores.RemoveRange(_context.GameScores);
                _context.Customers.RemoveRange(_context.Customers);
                await _context.SaveChangesAsync();

                return Ok("Đã xóa tất cả dữ liệu thành công");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi xóa dữ liệu: {ex.Message}");
            }
        }
    }
}
