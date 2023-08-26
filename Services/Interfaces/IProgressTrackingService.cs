using ArtisanELearningSystem.Entities;

namespace ArtisanELearningSystem.Services.Interfaces
{
    public interface IProgressTrackingService
    {
        Task<IEnumerable<UserProgress>> GetUserProgress(string userId, int courseId);
        Task MarkCourseElementCompleted(string userId, int courseId, int? lectureId = null, int? quizId = null, int? questionId = null);
    }
}
