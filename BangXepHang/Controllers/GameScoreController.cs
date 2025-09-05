using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BangXepHang.Data;
using BangXepHang.Models;

namespace BangXepHang.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameScoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GameScoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameScore>>> GetGameScores(int? customerId = null, int limit = 50)
        {
            var query = _context.GameScores
                .Include(gs => gs.Customer)
                .AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(gs => gs.CustomerId == customerId.Value);
            }

            var gameScores = await query
                .OrderByDescending(gs => gs.PlayTime)
                .Take(limit)
                .ToListAsync();

            return Ok(gameScores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameScore>> GetGameScore(int id)
        {
            var gameScore = await _context.GameScores
                .Include(gs => gs.Customer)
                .FirstOrDefaultAsync(gs => gs.Id == id);

            if (gameScore == null)
            {
                return NotFound();
            }

            return Ok(gameScore);
        }

        [HttpPost]
        public async Task<ActionResult<GameScore>> CreateGameScore(GameScore gameScore)
        {
            if (gameScore.CustomerId <= 0)
            {
                return BadRequest("CustomerId không hợp lệ");
            }

            if (gameScore.Score < 0)
            {
                return BadRequest("Điểm số không được âm");
            }

            // Kiểm tra khách hàng có tồn tại không
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == gameScore.CustomerId);
            if (!customerExists)
            {
                return BadRequest("Khách hàng không tồn tại");
            }

            // Nếu không có thời gian chơi, sử dụng thời gian hiện tại
            if (gameScore.PlayTime == default)
            {
                gameScore.PlayTime = DateTime.UtcNow;
            }

            _context.GameScores.Add(gameScore);
            await _context.SaveChangesAsync();

            // Load thông tin khách hàng để trả về
            await _context.Entry(gameScore)
                .Reference(gs => gs.Customer)
                .LoadAsync();

            return CreatedAtAction(nameof(GetGameScore), new { id = gameScore.Id }, gameScore);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGameScore(int id, GameScore gameScore)
        {
            if (id != gameScore.Id)
            {
                return BadRequest();
            }

            if (gameScore.Score < 0)
            {
                return BadRequest("Điểm số không được âm");
            }

            _context.Entry(gameScore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameScoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameScore(int id)
        {
            var gameScore = await _context.GameScores.FindAsync(id);
            if (gameScore == null)
            {
                return NotFound();
            }

            _context.GameScores.Remove(gameScore);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("stats/{customerId}")]
        public async Task<ActionResult<object>> GetCustomerStats(int customerId)
        {
            var stats = await _context.GameScores
                .Where(gs => gs.CustomerId == customerId)
                .GroupBy(gs => 1)
                .Select(g => new
                {
                    TotalScore = g.Sum(gs => gs.Score),
                    PlayCount = g.Count(),
                    AverageScore = g.Average(gs => gs.Score),
                    HighestScore = g.Max(gs => gs.Score),
                    LowestScore = g.Min(gs => gs.Score),
                    FirstPlayTime = g.Min(gs => gs.PlayTime),
                    LastPlayTime = g.Max(gs => gs.PlayTime)
                })
                .FirstOrDefaultAsync();

            if (stats == null)
            {
                return NotFound("Không tìm thấy dữ liệu cho khách hàng này");
            }

            return Ok(stats);
        }

        private bool GameScoreExists(int id)
        {
            return _context.GameScores.Any(e => e.Id == id);
        }
    }
}
