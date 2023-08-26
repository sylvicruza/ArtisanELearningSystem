using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Models;

namespace ArtisanELearningSystem.Services.Interfaces
{
    public interface IQuizService
    {
        Task<Quiz> GetQuizByIdAsync(int quizId);
        Task<List<Quiz>> GetQuizzesForCourseAsync(int courseId);
        Task CreateQuizAsync(Quiz quiz);
        Task UpdateQuizAsync(Quiz quiz);
        Task DeleteQuizAsync(string quizId);
        Task<List<string>> EvaluateQuizAsync(Quiz quiz, List<int> answerIds);
    }
}
