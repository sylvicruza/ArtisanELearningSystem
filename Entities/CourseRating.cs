using ArtisanELearningSystem.Utils;

namespace ArtisanELearningSystem.Entities
{
    public class CourseRating : TimeHelper
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int Rating { get; set; }

        public string? Comment { get; set; }
        public DateTime DateCreated { get; set; }

        public string? TimeAgo
        {
            get { return DateTimeHelper.TimeTicks((DateTime)DateCreated); }
        }
    }
}
