using Microsoft.EntityFrameworkCore;
using QuizApplicationAPI.Models;

namespace QuizApplicationAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<AnswerOption> AnswerOption { get; set; }
        public DbSet<QuizAttempt> QuizAttempt { get; set; }
        public DbSet<UserAnswer> UserAnswer { get; set; }
    }
}
