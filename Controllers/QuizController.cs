using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApplicationAPI.Data;
using QuizApplicationAPI.DTOs.Request;
using QuizApplicationAPI.DTOs.Response.Answer;
using QuizApplicationAPI.DTOs.Response.Quiz;
using QuizApplicationAPI.DTOs.Responsse;
using QuizApplicationAPI.Models;

namespace QuizApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<QuizListResponse>>> GetQuizList(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "CreatedAt",
            [FromQuery] bool ascending = true)
        {
            // Validate and normalize pagination parameters
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50;

            var query = _context.Quiz.AsQueryable();
            
            // Apply dynamic sorting based on sortBy parameter
            query = sortBy.ToLower() switch
            {
                "title" => ascending ? query.OrderBy(q => q.Title) : query.OrderByDescending(q => q.Title),
                "createdat" => ascending ? query.OrderBy(q => q.CreatedAt) : query.OrderByDescending(q => q.CreatedAt),
                _ => query.OrderBy(q => q.CreatedAt)
            };

            // Apply pagination
            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new QuizListResponse
                {
                    QuizId = q.QuizId,
                    Title = q.Title,
                    CreatedAt = q.CreatedAt,
                })
                .ToListAsync();

            if (!items.Any())
                return NotFound("No quizzes found.");

            return Ok(new PagedResponse<QuizListResponse>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDetailResponse>> GetQuizById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid quiz ID.");

            var quiz = await _context.Quiz
                .Where(q => q.QuizId == id)
                .Select(q => new QuizDetailResponse
                {
                    QuizId = q.QuizId,
                    Title = q.Title,
                    DescriptionQuiz = q.DescriptionQuiz,
                    PassingScorePercent = q.PassingScorePercent,
                    TimeLimitMinutes = q.TimeLimitMinutes,
                })
                .FirstOrDefaultAsync();

            if (quiz == null)
                return NotFound($"Quiz with ID {id} not found.");

            return Ok(quiz);
        }

        [HttpGet("{quizId}/questions")]
        public async Task<ActionResult<List<QuestionResponse>>> GetQuestionsForQuiz(int quizId)
        {
            if (quizId <= 0)
                return BadRequest("Invalid quiz ID.");

            var questions = await _context.Question
                .Where(q => q.QuizId == quizId)
                .Include(q => q.AnswerOptions)
                .OrderBy(q => q.QuestionOrder)
                .Select(q => new QuestionResponse
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    QuestionOrder = q.QuestionOrder,
                    TimeAllowedSeconds = q.TimeAllowedSeconds,
                    AnswerOptions = q.AnswerOptions.Select(a => new AnswerOptionResponse
                    {
                        AnswerOptionId = a.AnswerOptionId,
                        AnswerText = a.AnswerText
                    }).ToList()
                })
                .ToListAsync();

            if (!questions.Any())
                return NotFound("No questions found for this quiz.");

            return Ok(questions);
        }

        [HttpPost("quiz-attempt")]
        public async Task<ActionResult<StartQuizResponse>> StartQuiz([FromBody] StartQuizRequest request)
        {
            try
            {
                // Validate input and check quiz existence
                if (request == null || request.QuizId <= 0)
                    return BadRequest("Invalid QuizId.");

                var quiz = await _context.Quiz.FindAsync(request.QuizId);
                if (quiz == null)
                    return NotFound("Quiz not found");

                // Create new quiz attempt with start time
                var attempt = new QuizAttempt
                {
                    QuizId = request.QuizId,
                    StartTime = DateTime.UtcNow,
                };

                _context.QuizAttempt.Add(attempt);
                await _context.SaveChangesAsync();

                return Ok(new StartQuizResponse
                {
                    QuizAttemptId = attempt.QuizAttemptId,
                    StartTime = attempt.StartTime,
                    TimeLimitMinutes = quiz.TimeLimitMinutes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("attempts/{quizAttemptId}/result")]
        public async Task<ActionResult<SubmitQuizResponse>> SubmitQuiz(Guid quizAttemptId)
        {
            try
            {
                // Validate quiz attempt and retrieve related data
                if (quizAttemptId == Guid.Empty)
                    return BadRequest("Invalid QuizAttemptId.");

                var attempt = await _context.QuizAttempt
                    .FirstOrDefaultAsync(a => a.QuizAttemptId == quizAttemptId);

                if (attempt == null)
                    return NotFound("Quiz attempt not found");

                var quiz = await _context.Quiz
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(q => q.QuizId == attempt.QuizId);

                if (quiz == null)
                    return NotFound("Quiz not found");

                // Get and validate user answers
                var userAnswers = await _context.UserAnswer
                    .Where(ua => ua.QuizAttemptId == quizAttemptId)
                    .ToListAsync();

                if (userAnswers.Count != quiz.Questions.Count)
                    return BadRequest("Please answer all questions before submitting.");

                // Calculate quiz results and prepare detailed response
                int totalQuestions = quiz.Questions.Count;
                int correctAnswers = 0;
                var answerDetails = new List<AnswerDetailResponse>();

                foreach (var question in quiz.Questions)
                {
                    var userAnswer = userAnswers.FirstOrDefault(ua => ua.QuestionId == question.QuestionId);
                    if (userAnswer == null) continue;

                    var correctOption = await _context.AnswerOption
                        .FirstOrDefaultAsync(ao => ao.QuestionId == question.QuestionId && ao.IsCorrect);

                    if (correctOption == null) continue;

                    bool isCorrect = userAnswer.AnswerOptionId == correctOption.AnswerOptionId;
                    if (isCorrect) correctAnswers++;

                    // Build detailed answer information for each question
                    answerDetails.Add(new AnswerDetailResponse
                    {
                        QuestionId = question.QuestionId,
                        QuestionText = question.QuestionText,
                        SelectedOptionId = userAnswer.AnswerOptionId,
                        Explain = question.Explain,
                        Options = await _context.AnswerOption
                            .Where(ao => ao.QuestionId == question.QuestionId)
                            .Select(ao => new AnswerOptionDetailResponse
                            {
                                AnswerOptionId = ao.AnswerOptionId,
                                AnswerText = ao.AnswerText,
                                IsCorrect = ao.IsCorrect
                            })
                            .ToListAsync()
                    });
                }

                // Calculate final score and update attempt status
                attempt.EndTime = DateTime.UtcNow;
                attempt.TotalCorrect = correctAnswers;

                decimal passingScorePercent = quiz.PassingScorePercent;
                decimal scorePercent = ((decimal)correctAnswers / totalQuestions) * 100;
                bool passed = scorePercent >= passingScorePercent;
                attempt.Passed = passed;

                await _context.SaveChangesAsync();

                var duration = attempt.EndTime.Value - attempt.StartTime;

                return Ok(new SubmitQuizResponse
                {
                    TotalQuestions = totalQuestions,
                    CorrectAnswers = correctAnswers,
                    DurationInSeconds = (int)duration.TotalSeconds,
                    Passed = passed,
                    Answers = answerDetails
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("attempts/{quizAttemptId}/answers")]
        public async Task<ActionResult<CheckAnswerResponse>> CheckAnswer([FromBody] CheckAnswerRequest request)
        {
            try
            {
                // Validate request parameters
                if (request == null)
                    return BadRequest("Invalid request.");

                if (request.QuizAttemptId == Guid.Empty)
                    return BadRequest("Invalid QuizAttemptId.");

                if (request.QuestionId <= 0 || request.SelectedOptionId <= 0)
                    return BadRequest("Invalid QuestionId or SelectedOptionId.");

                // Validate quiz attempt status
                var attempt = await _context.QuizAttempt
                    .FirstOrDefaultAsync(a => a.QuizAttemptId == request.QuizAttemptId);

                if (attempt == null)
                    return NotFound("Quiz attempt not found.");

                if (attempt.EndTime != null)
                    return BadRequest("This quiz attempt has already been submitted.");

                // Validate question and check answer
                var question = await _context.Question
                    .Include(q => q.AnswerOptions)
                    .FirstOrDefaultAsync(q => q.QuestionId == request.QuestionId);

                if (question == null)
                    return NotFound("Question not found.");

                if (question.QuizId != attempt.QuizId)
                    return BadRequest("Question does not belong to this quiz.");

                var correctOption = question.AnswerOptions.FirstOrDefault(o => o.IsCorrect);
                if (correctOption == null)
                    return StatusCode(500, "No correct answer found for this question.");

                // Process answer and save user response
                bool isCorrect = request.SelectedOptionId == correctOption.AnswerOptionId;

                var userAnswer = new UserAnswer
                {
                    QuizAttemptId = request.QuizAttemptId,
                    QuestionId = request.QuestionId,
                    AnswerOptionId = request.SelectedOptionId
                };
                _context.UserAnswer.Add(userAnswer);
                await _context.SaveChangesAsync();

                return Ok(new CheckAnswerResponse
                {
                    IsCorrect = isCorrect,
                    Explain = question.Explain,
                    CorrectOptionId = correctOption.AnswerOptionId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
