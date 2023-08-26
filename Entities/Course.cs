using ArtisanELearningSystem.Utils;

namespace ArtisanELearningSystem.Entities
{
    public class Course : TimeHelper
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string LearningOutcomes { get; set; }

        public string Requirements { get; set; }

        public string Level { get; set; }

        public string Category { get; set; }

        public ICollection<Lecture> Lectures { get; set; }
        public ICollection<Quiz> Quizs { get; set; }

        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }

        public bool IsLoginRequired { get; set; }

        public bool IsPublished { get; set; }
        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; } //Author

        public DateTime? DateCreated { get; set; }

        public ICollection<CourseEnrollment> CourseEnrollments { get; set; }

        public string? Poster { get; set; } //Course Poster

        public double? AverageRating { get; set; }
        public int? TotalRatings { get; set; }

        public string? Badge { get; set; }

        public int? ViewCount { get; set; }


        public string? Tags { get; set; }

        public string? TimeAgo
        {
            get { return DateTimeHelper.TimeTicks((DateTime)DateCreated); }
        }


    }
}
