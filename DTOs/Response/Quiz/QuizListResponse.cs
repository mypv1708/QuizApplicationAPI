namespace QuizApplicationAPI.DTOs.Responsse
{
    public class QuizListResponse
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}