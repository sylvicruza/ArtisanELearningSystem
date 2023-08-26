using ArtisanELearningSystem.Entities;

namespace ArtisanELearningSystem.Models
{
    public class QuizViewModel
    {
  
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public string? Image { get; set; }

        public List<Question> Questions { get; set; } 
        public string QuestionTitle { get; set; }
        public string? Score { get; set; }

        public string OptionTitle { get; set; }
        public bool IsCorrectAnswer { get; set; }

        public int CourseId { get; set; }

        public List<Options>? Options { get; set; }

    }
}
