using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BangXepHang.Data;
using BangXepHang.Models.DTOs;

namespace BangXepHang.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RankingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RankingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("monthly")]
        public async Task<ActionResult<MonthlyRankingResponse>> GetMonthlyRanking(
            int? year = null, 
            int? month = null, 
            int limit = 10)
        {
            var currentDate = DateTime.UtcNow;
            var targetYear = year ?? currentDate.Year;
            var targetMonth = month ?? currentDate.Month;

            if (targetMonth < 1 || targetMonth > 12)
            {
                return BadRequest("Tháng phải từ 1 đến 12");
            }

            if (targetYear < 2000 || targetYear > 2100)
            {
                return BadRequest("Năm không hợp lệ");
            }

            var startDate = new DateTime(targetYear, targetMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

            var rankings = await _context.GameScores
                .Where(gs => gs.PlayTime >= startDate && gs.PlayTime <= endDate)
                .Include(gs => gs.Customer)
                .GroupBy(gs => gs.CustomerId)
                .Select(g => new RankingResponse
                {
                    CustomerId = g.Key,
                    CustomerName = g.First().Customer.Name,
                    CustomerAvatar = g.First().Customer.Avatar,
                    TotalScore = g.Sum(gs => gs.Score),
                    PlayCount = g.Count(),
                    LastPlayTime = g.Max(gs => gs.PlayTime)
                })
                .OrderByDescending(r => r.TotalScore)
                .ThenByDescending(r => r.LastPlayTime)
                .Take(limit)
                .ToListAsync();

            for (int i = 0; i < rankings.Count; i++)
            {
                rankings[i].Rank = i + 1;
            }

            var monthNames = new[] { "", "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
                                   "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };

            var response = new MonthlyRankingResponse
            {
                Year = targetYear,
                Month = targetMonth,
                MonthName = monthNames[targetMonth],
                Rankings = rankings,
                TotalPlayers = await _context.GameScores
                    .Where(gs => gs.PlayTime >= startDate && gs.PlayTime <= endDate)
                    .Select(gs => gs.CustomerId)
                    .Distinct()
                    .CountAsync()
            };

            return Ok(response);
        }


        [HttpGet("overall")]
        public async Task<ActionResult<List<RankingResponse>>> GetOverallRanking(int limit = 10)
        {
            var rankings = await _context.GameScores
                .Include(gs => gs.Customer)
                .GroupBy(gs => gs.CustomerId)
                .Select(g => new RankingResponse
                {
                    CustomerId = g.Key,
                    CustomerName = g.First().Customer.Name,
                    CustomerAvatar = g.First().Customer.Avatar,
                    TotalScore = g.Sum(gs => gs.Score),
                    PlayCount = g.Count(),
                    LastPlayTime = g.Max(gs => gs.PlayTime)
                })
                .OrderByDescending(r => r.TotalScore)
                .ThenByDescending(r => r.LastPlayTime)
                .Take(limit)
                .ToListAsync();

            for (int i = 0; i < rankings.Count; i++)
            {
                rankings[i].Rank = i + 1;
            }

            return Ok(rankings);
        }

        [HttpGet("available-months")]
        public async Task<ActionResult<List<object>>> GetAvailableMonths()
        {
            var months = await _context.GameScores
                .Select(gs => new { gs.PlayTime.Year, gs.PlayTime.Month })
                .Distinct()
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .Select(x => new { x.Year, x.Month })
                .ToListAsync();

            return Ok(months);
        }

        [HttpGet("by-date-range")]
        public async Task<ActionResult<object>> GetRankingByDateRange(
            [FromQuery] string startDate, 
            [FromQuery] string endDate, 
            int limit = 10)
        {
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return BadRequest("Ngày bắt đầu và ngày kết thúc không được để trống");
            }

            if (!DateTime.TryParse(startDate, out var start) || !DateTime.TryParse(endDate, out var end))
            {
                return BadRequest("Định dạng ngày không hợp lệ. Sử dụng format: yyyy-MM-dd");
            }

            if (start > end)
            {
                return BadRequest("Ngày bắt đầu không được lớn hơn ngày kết thúc");
            }

            if (start > DateTime.UtcNow)
            {
                return BadRequest("Ngày bắt đầu không được lớn hơn ngày hiện tại");
            }

            // Set time to start of day and end of day
            start = start.Date;
            end = end.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            // Get rankings for the specified period
            var rankings = await _context.GameScores
                .Where(gs => gs.PlayTime >= start && gs.PlayTime <= end)
                .Include(gs => gs.Customer)
                .GroupBy(gs => gs.CustomerId)
                .Select(g => new RankingResponse
                {
                    CustomerId = g.Key,
                    CustomerName = g.First().Customer.Name,
                    CustomerAvatar = g.First().Customer.Avatar,
                    TotalScore = g.Sum(gs => gs.Score),
                    PlayCount = g.Count(),
                    LastPlayTime = g.Max(gs => gs.PlayTime)
                })
                .OrderByDescending(r => r.TotalScore)
                .ThenByDescending(r => r.LastPlayTime)
                .Take(limit)
                .ToListAsync();

            // Add rank numbers
            for (int i = 0; i < rankings.Count; i++)
            {
                rankings[i].Rank = i + 1;
            }

            var response = new
            {
                Period = $"Từ {start:dd/MM/yyyy} đến {end:dd/MM/yyyy}",
                StartDate = start,
                EndDate = end,
                Rankings = rankings,
                TotalPlayers = await _context.GameScores
                    .Where(gs => gs.PlayTime >= start && gs.PlayTime <= end)
                    .Select(gs => gs.CustomerId)
                    .Distinct()
                    .CountAsync()
            };

            return Ok(response);
        }
    }
}
