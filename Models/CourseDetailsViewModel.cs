using ArtisanELearningSystem.Entities;

namespace ArtisanELearningSystem.Models
{
    public class CourseDetailsViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<UserProgress> UserProgress { get; set; }
    }

}
