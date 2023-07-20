namespace ArtisanELearningSystem.Entities
{
    public class Lecture
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsPreviewFree { get; set; }
        public string? VideoType { get; set; }
        public string? URL { get; set; } //path
        public string? Runtime { get; set; } //hh:mm:ss

        public string? Attachment { get; set; }

    }

}
