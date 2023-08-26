using System.ComponentModel.DataAnnotations.Schema;

namespace ArtisanELearningSystem.Entities
{
    public class Student : UserAbstractEntity
    {
        public string? profileImage { get; set; }
        public string? StudentId { get; set; }
        public string? Mobile { get; set; }
        public string? AboutMe { get; set; }

        public ICollection<CourseEnrollment> CourseEnrollments { get; set; }
        public ICollection<CourseRating> CourseRatings { get; set; }

        public string? Interests { get; set; }

        public int? PreferredDifficultyLevel { get; set; }

        public string? LearningObjectives { get; set; }
    }

}
