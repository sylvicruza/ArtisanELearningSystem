using ArtisanELearningSystem.Data;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace ArtisanELearningSystem.Services
{
    public class DiscussionService : IDiscussionService
    {
        private readonly ArtisanELearningSystemContext _context;
      


        public DiscussionService(ArtisanELearningSystemContext context)
        {
            _context = context;
          

        }

        public async Task<List<Discussion>> GetDiscussionsForCourse(int courseId)
        {
            return await _context.Discussion
                .Where(d => d.CourseId == courseId)
                .OrderByDescending(d => d.Timestamp)
                .ToListAsync();
        }

        public async Task AddNewPost(Discussion discussion)
        {
            _context.Discussion.Add(discussion);
            await _context.SaveChangesAsync();
        }
    }
}
