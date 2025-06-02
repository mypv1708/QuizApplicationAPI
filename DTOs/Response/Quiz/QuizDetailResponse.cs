namespace QuizApplicationAPI.DTOs.Responsse
{
    public class QuizDetailResponse
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string DescriptionQuiz { get; set; }
        public decimal PassingScorePercent { get; set; }
        public int? TimeLimitMinutes { get; set; }
    }
} 