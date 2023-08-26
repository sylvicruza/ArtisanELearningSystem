namespace ArtisanELearningSystem.Entities
{
    public class UserProgress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int? LectureId { get; set; }
        public Lecture? Lecture { get; set; }
        public int? QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public int? QuestionId { get; set; }
        public Question? Question { get; set; }
        public bool IsCompleted { get; set; }
    }
}
