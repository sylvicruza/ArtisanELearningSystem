namespace ArtisanELearningSystem.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? LectureId { get; set; }
        public virtual Lecture? Lecture { get; set; }
        public int? QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }



    }

}
