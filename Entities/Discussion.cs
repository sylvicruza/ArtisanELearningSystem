namespace ArtisanELearningSystem.Entities
{
    public class Discussion
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        // Other properties as needed
    }
}
