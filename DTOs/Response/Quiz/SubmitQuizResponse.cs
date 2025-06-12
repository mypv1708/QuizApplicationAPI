namespace QuizApplicationAPI.DTOs.Response.Quiz
{
    public class SubmitQuizResponse
    {
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int DurationInSeconds { get; set; }
        public bool Passed { get; set; }
        public List<AnswerDetailResponse> Answers { get; set; }
    }

    public class AnswerDetailResponse
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int SelectedOptionId { get; set; }
        public string Explain { get; set; }
        public List<AnswerOptionDetailResponse> Options { get; set; }
    }

    public class AnswerOptionDetailResponse
    {
        public int AnswerOptionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
} 