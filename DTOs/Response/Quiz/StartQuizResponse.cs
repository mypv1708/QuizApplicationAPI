namespace QuizApplicationAPI.DTOs.Responsse
{
    public class StartQuizResponse
    {
        public Guid QuizAttemptId { get; set; }
        public DateTime StartTime { get; set; }
        public int? TimeLimitMinutes { get; set; }
    }
} 