using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizApplicationAPI.Models
{
    public class QuizAttempt
    {
        [Key]
        public Guid QuizAttemptId { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        //public int? TotalTimeTaken { get; set; } 
        public int TotalCorrect { get; set; }
        public bool? Passed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
