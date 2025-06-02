namespace QuizApplicationAPI.DTOs.Request
{
    public class CheckAnswerRequest
    {
        public Guid QuizAttemptId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
    }
} 