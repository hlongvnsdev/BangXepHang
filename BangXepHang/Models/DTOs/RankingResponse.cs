namespace BangXepHang.Models.DTOs
{
    public class RankingResponse
    {
        public int Rank { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerAvatar { get; set; }
        public int TotalScore { get; set; }
        public int PlayCount { get; set; }
        public DateTime LastPlayTime { get; set; }
    }

    public class MonthlyRankingResponse
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public List<RankingResponse> Rankings { get; set; } = new List<RankingResponse>();
        public int TotalPlayers { get; set; }
    }
}
