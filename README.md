# Quiz Application API

## Overview

This is a Quiz Application API built with ASP.NET Core 8.0. The API provides endpoints for managing quizzes, questions, and user interactions with quizzes.

### Features

- Quiz management (CRUD operations)
- Question management with multiple choice answers
- Quiz taking functionality
- Answer submission and scoring
- User progress tracking

### API Endpoints

#### Quiz Management

- `GET /api/quiz/list` - Get list of all quizzes
- `GET /api/quiz/get/{id}` - Get quiz details by ID
- `POST /api/quiz/create` - Create a new quiz
- `PUT /api/quiz/update` - Update an existing quiz
- `DELETE /api/quiz/delete/{id}` - Delete a quiz

#### Question Management

- `GET /api/question/list` - Get list of all questions
- `GET /api/question/get/{id}` - Get question details by ID
- `POST /api/question/create` - Create a new question
- `PUT /api/question/update` - Update an existing question
- `DELETE /api/question/delete/{id}` - Delete a question

#### Quiz Taking

- `POST /api/quiz/start` - Start a new quiz session
- `POST /api/quiz/answer` - Submit an answer for a question
- `GET /api/quiz/submit-answers` - Submit all answers and get results

### Data Models

#### Quiz

- Id (int)
- Title (string)
- Description (string)
- TimeLimit (int) - in minutes
- CreatedAt (DateTime)
- UpdatedAt (DateTime)

#### Question

- Id (int)
- QuizId (int)
- QuestionText (string)
- Options (List<string>)
- CorrectAnswer (int)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)

#### QuizSession

- Id (int)
- QuizId (int)
- UserId (int)
- StartTime (DateTime)
- EndTime (DateTime)
- Score (int)
- Status (string)

### Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- AutoMapper
- Swagger/OpenAPI
