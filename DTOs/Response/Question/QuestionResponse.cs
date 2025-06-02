namespace QuizApplicationAPI.DTOs.Responsse
{
    public class QuestionResponse
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int QuestionOrder { get; set; }
        public int TimeAllowedSeconds { get; set; }
        public List<AnswerOptionResponse> AnswerOptions { get; set; }
    }
}
