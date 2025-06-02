using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizApplicationAPI.Models
{
    [Table("Question")]
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public string QuestionText { get; set; }
        public int QuestionOrder { get; set; }
        public int TimeAllowedSeconds { get; set; }
        public string Explain { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<AnswerOption> AnswerOptions { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
