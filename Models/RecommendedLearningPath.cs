using ArtisanELearningSystem.Entities;

namespace ArtisanELearningSystem.Models
{
    public class RecommendedLearningPath
    {
        public List<Course> Courses { get; set; }
        public double Score { get; set; }
    }
}
