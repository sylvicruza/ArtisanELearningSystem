using ArtisanELearningSystem.Data;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Models;
using ArtisanELearningSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtisanELearningSystem.Services
{
    public class QuizService : IQuizService
    {
        private readonly ArtisanELearningSystemContext _context;


        public QuizService(ArtisanELearningSystemContext context)
        {
            _context = context;

        }

        public async Task<Quiz> GetQuizByIdAsync(int quizId)
        {
            return await _context.Quiz
                .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }

        public async Task<List<Quiz>> GetQuizzesForCourseAsync(int courseId)
        {
            return await _context.Quiz
                 .Include(a => a.Course)
                .Include(a => a.Questions)
                    .ThenInclude(q => q.Options)
                .Where(q => q.CourseId == courseId)
                .ToListAsync();
        }

        public async Task CreateQuizAsync(Quiz model)
        {
            // Create a new Quiz instance
            var quiz = new Quiz
            {
                Title = model.Title,
                Description = model.Description,
                CourseId = model.CourseId,
                Questions = new List<Question>() // Initialize the list of questions
            };
            Quiz newQuiz = new Quiz();
            newQuiz.CourseId = quiz.CourseId; newQuiz.Title = quiz.Title; newQuiz.Description = quiz.Description;
           Quiz newAddedQuiz = _context.Quiz.Add(newQuiz).Entity;

            foreach (var question in model.Questions)
            {
                var newQuestion = new Question
                {
                    Title = question.Title,
                    Type = question.Type,
                    Score = question.Score,
                    QuizId = newAddedQuiz.Id,
                    
                    // ... Other properties ...
                    Options = new List<Options>() // Initialize the list of options
                };
              Question newAddedQuestion =  _context.Question.Add(newQuestion).Entity; // Add the question to the context

                foreach (var option in question.Options)
                {
                    var newOption = new Options
                    {
                        Title = option.Title,
                        IsCorrectAnswer = option.IsCorrectAnswer,
                        QuestionId = newAddedQuestion.Id,
                        
                        // ... Other properties ...
                    };

                   // newQuestion.Options.Add(newOption); // Add the option to the question
                    _context.Options.Add(newOption);    // Add the option to the context
                }

              //  quiz.Questions.Add(newQuestion); // Add the question to the quiz
               
            }
        

            // Add the quiz to the context
            await _context.SaveChangesAsync(); // Save changes to the database
           
        }



        public async Task UpdateQuizAsync(Quiz quiz)
        {
            _context.Quiz.Update(quiz);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuizAsync(string quizId)
        {
            var quiz = await _context.Quiz.FindAsync(quizId);
            if (quiz != null)
            {
                _context.Quiz.Remove(quiz);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<string>> EvaluateQuizAsync(Quiz quiz, List<int> answerIds)
        {
            // Fetch the quiz from the context
            quiz = await _context.Quiz
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == quiz.Id);

            if (quiz == null)
            {
                return null; // Quiz not found
            }

            double totalScore = 0;
            double maxPossibleScore = quiz.Questions.Sum(q => double.Parse(q.Score));

            foreach (var question in quiz.Questions)
            {
                var selectedOptionId = answerIds.FirstOrDefault(id => id == question.Id);

                if (selectedOptionId != default)
                {
                    var selectedOption = question.Options.FirstOrDefault(o => o.Id == selectedOptionId);

                    if (selectedOption != null && selectedOption.IsCorrectAnswer)
                    {
                        totalScore += double.Parse(question.Score);
                    }
                }
            }

            double percentageScore = (totalScore / maxPossibleScore) * 100;

            var feedback = new List<string>
    {
        $"Quiz: {quiz.Title}",
        $"Your Score: {totalScore} / {maxPossibleScore}",
        $"Percentage Score: {percentageScore}%"
    };

            return feedback;
        }


    }
}
