using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Models;

namespace ArtisanELearningSystem.Services.Interfaces
{
    public interface IRecommendationService
    {
        List<RecommendedLearningPath> GetPersonalizedLearningPaths(Student user, int numberOfPaths = 5);
    }
}
