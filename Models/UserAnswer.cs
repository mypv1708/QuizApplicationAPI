using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizApplicationAPI.Models
{
    public class UserAnswer
    {
        [Key]
        public Guid UserAnswerId { get; set; }
        public Guid QuizAttemptId { get; set; }
        public QuizAttempt QuizAttempt { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int AnswerOptionId { get; set; }
        public AnswerOption AnswerOption { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
