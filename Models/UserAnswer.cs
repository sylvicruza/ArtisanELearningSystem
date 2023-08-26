namespace ArtisanELearningSystem.Models
{
    public class UserAnswer
    {
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
    }
    public class QuizEvaluationResult
    {
        public string QuizTitle { get; set; }
        public double UserScore { get; set; }
        public double MaxPossibleScore { get; set; }
        public double PercentageScore { get; set; }
    }
}
