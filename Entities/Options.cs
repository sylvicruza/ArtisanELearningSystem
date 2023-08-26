namespace ArtisanELearningSystem.Entities
{
    public class Options
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCorrectAnswer { get; set; }

        public int? QuestionId { get; set; }
        public Question? Question { get; set; }
    }

}
