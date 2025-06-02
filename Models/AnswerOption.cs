using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizApplicationAPI.Models
{
    [Table("AnswerOption")]
    public class AnswerOption
    {
        [Key]
        public int AnswerOptionId { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
