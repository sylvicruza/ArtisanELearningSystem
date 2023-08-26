namespace ArtisanELearningSystem.Models
{
    public class RatingData
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public double Rating { get; set; }
    }

    public class RatingPrediction
    {
        public float Score { get; set; }
    }
}
