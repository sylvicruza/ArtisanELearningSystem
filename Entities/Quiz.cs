namespace ArtisanELearningSystem.Entities
{
    public class Quiz
    {

        public int Id { get; set; }

   
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Question>? Questions { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }



    }

}
