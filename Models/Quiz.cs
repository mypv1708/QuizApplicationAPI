using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizApplicationAPI.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string? DescriptionQuiz { get; set; }
        public decimal PassingScorePercent { get; set; }
        public int? TimeLimitMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<QuizAttempt> QuizAttempts { get; set; }
    }
}
