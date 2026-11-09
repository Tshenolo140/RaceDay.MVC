namespace RaceDay.MVC.Models
{
    public class ResultViewModel
    {
        public int ResultId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public TimeSpan FinishTime { get; set; }
        public int OverallRank { get; set; }
        public int CategoryRank { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}