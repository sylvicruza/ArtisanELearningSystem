using ArtisanELearningSystem.Entities;

namespace ArtisanELearningSystem.Services.Interfaces
{
    public interface IDiscussionService
    {
        Task<List<Discussion>> GetDiscussionsForCourse(int courseId);
        Task AddNewPost(Discussion discussion);
    }
}
