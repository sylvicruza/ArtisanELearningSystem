using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ArtisanELearningSystem.Models
{
    public class LectureViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPreviewFree { get; set; }
        public string? VideoType { get; set; }
        public string? URL { get; set; } //path
        public string? hours { get; set; } //hh
        public string? mins { get; set; } //:mm
        public string? secs { get; set; } //:ss

        public string? Attachment { get; set; }

        // Property to hold the uploaded video file
        [Display(Name = "Video File")]
        public IFormFile? VideoFile { get; set; }
        public IFormFile? PosterFile { get; set; }

        public string? Poster { get; set; }

        public int CourseId { get; set; }

        public int InstrustorId { get; set; }
    }
}
