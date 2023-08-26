using ArtisanELearningSystem.Entities;

namespace ArtisanELearningSystem.Models
{
    public class CourseDiscussionViewModel
    {
        public int CourseId { get; set; }
        public List<Discussion> Discussions { get; set; }
        public string NewPostContent { get; set; }
    }
}
