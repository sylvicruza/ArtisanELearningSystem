namespace ArtisanELearningSystem.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string LearningOutcomes { get; set; }

        public string Requirements { get; set; }

        public string Level { get; set; }

        public string Category { get; set; }

        public int CurriculumId { get; set; }
        public virtual Curriculum Curriculum { get; set; }

        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }

        public bool? IsLoginRequired { get; set; }

        public bool IsPublished { get; set; }
        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; } //Author

        public DateTime? DateCreated { get; set; }
       
    }
}
