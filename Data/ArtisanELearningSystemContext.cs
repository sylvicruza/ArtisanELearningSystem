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
    }
}
