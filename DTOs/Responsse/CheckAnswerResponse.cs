namespace QuizApplicationAPI.DTOs.Responsse
{
    public class CheckAnswerResponse
    {
        public bool IsCorrect { get; set; }
        public string Explain { get; set; }
        public int CorrectOptionId { get; set; }
    }
} 