namespace QuizApplicationAPI.DTOs.Response.Answer
{
    public class CheckAnswerResponse
    {
        public bool IsCorrect { get; set; }
        public string Explain { get; set; }
        public int CorrectOptionId { get; set; }
    }
} 