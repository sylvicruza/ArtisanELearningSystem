namespace ArtisanELearningSystem.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public string? Image { get; set; }
        public string Title { get; set; }
        public string? Score { get; set; }

        public List<Options>? Options { get; set; }

        public int? QuizId { get; set; }
        public Quiz? Quiz { get; set; }
    }

}
