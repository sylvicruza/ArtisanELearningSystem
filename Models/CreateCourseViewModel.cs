using ArtisanELearningSystem.Entities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ArtisanELearningSystem.Models
{
    public class CreateCourseViewModel
    {

        public string Title { get; set; }
        public string Description { get; set; }

        public string LearningOutcomes { get; set; }

        public string Requirements { get; set; }

        public string Level { get; set; }

        public string Category { get; set; }

        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }

        public bool IsLoginRequired { get; set; }

        public bool IsPublished { get; set; }

        public string? CreatedBy { get; set; }

        public ICollection<string> Tags { get; set; }


        public string Poster { get; set; }

        public string? Badge { get; set; }


        [Display(Name = "File")]
        public IFormFile? File { get; set; }

        public string TagsString
        {
            get { return string.Join(",", Tags); }
            set { Tags = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); }
        }

    }
}
