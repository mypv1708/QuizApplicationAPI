# Quiz Application API

## Overview

This is a Quiz Application API built with ASP.NET Core 8.0. The API provides endpoints for managing quizzes, questions, and quiz attempts.

### Features

- Quiz listing with pagination and sorting
- Quiz details retrieval
- Question management for quizzes
- Quiz attempt functionality
- Answer submission and scoring
- Detailed quiz results with explanations

### API Endpoints

#### Quiz Management

- `GET /api/quiz` - Get list of quizzes with pagination and sorting
  - Query parameters:
    - page (default: 1)
    - pageSize (default: 10, max: 50)
    - sortBy (default: "CreatedAt")
    - ascending (default: true)
- `GET /api/quiz/{id}` - Get quiz details by ID
- `GET /api/quiz/{quizId}/questions` - Get all questions for a specific quiz

#### Quiz Taking

- `POST /api/quiz/quiz-attempt` - Start a new quiz attempt
- `POST /api/quiz/attempts/{quizAttemptId}/answers` - Submit an answer for a question
- `GET /api/quiz/attempts/{quizAttemptId}/result` - Submit quiz and get detailed results

### Data Models

#### Quiz

- QuizId (int)
- Title (string)
- DescriptionQuiz (string)
- PassingScorePercent (decimal)
- TimeLimitMinutes (int)

#### Question

- QuestionId (int)
- QuizId (int)
- QuestionText (string)
- QuestionOrder (int)
- TimeAllowedSeconds (int)
- Explain (string)

#### AnswerOption

- AnswerOptionId (int)
- QuestionId (int)
- AnswerText (string)
- IsCorrect (boolean)

#### QuizAttempt

- QuizAttemptId (Guid)
- QuizId (int)
- StartTime (DateTime)
- EndTime (DateTime?)
- TotalCorrect (int)
- Passed (boolean)

#### UserAnswer

- QuizAttemptId (Guid)
- QuestionId (int)
- AnswerOptionId (int)

### Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
