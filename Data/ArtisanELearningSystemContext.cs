using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ArtisanELearningSystem.Entities;

namespace ArtisanELearningSystem.Data
{
    public class ArtisanELearningSystemContext : DbContext
    {
        public ArtisanELearningSystemContext (DbContextOptions<ArtisanELearningSystemContext> options)
            : base(options)
        {
        }

        public DbSet<ArtisanELearningSystem.Entities.Student> Student { get; set; } = default!;

        public DbSet<ArtisanELearningSystem.Entities.Instructor>? Instructor { get; set; }

        public DbSet<ArtisanELearningSystem.Entities.Course>? Course { get; set; }

        public DbSet<ArtisanELearningSystem.Entities.Section>? Section { get; set; }

        public DbSet<ArtisanELearningSystem.Entities.Lecture>? Lecture { get; set; }

        public DbSet<ArtisanELearningSystem.Entities.UserProgress>? UserProgress { get; set; }

        public DbSet<ArtisanELearningSystem.Entities.CourseEnrollment>? CourseEnrollment { get; set; }

        public DbSet<ArtisanELearningSystem.Entities.Discussion>? Discussion { get; set; }

        public DbSet<ArtisanELearningSystem.Entities.CourseRating>? CourseRating { get; set; }

        public DbSet<ArtisanELearningSystem.Entities.Quiz>? Quiz { get; set; }

        public DbSet<ArtisanELearningSystem.Entities.Question>? Question { get; set; }

        public DbSet<ArtisanELearningSystem.Entities.Options>? Options { get; set; }
    }
}
