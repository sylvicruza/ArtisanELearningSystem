using ArtisanELearningSystem.Data;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtisanELearningSystem.Services
{
    public class ProgressTrackingService : IProgressTrackingService
    {
        private readonly ArtisanELearningSystemContext _context;


        public ProgressTrackingService(ArtisanELearningSystemContext context)
        {
            _context = context;

        }

        public async Task MarkCourseElementCompleted(string userId, int courseId, int? lectureId = null, int? quizId = null, int? questionId = null)
        {
            var progress = await _context.UserProgress.FirstOrDefaultAsync(p =>
                p.UserId == userId &&
                p.CourseId == courseId &&
                p.LectureId == lectureId &&
                p.QuizId == quizId &&
                p.QuestionId == questionId);

            if (progress == null)
            {
                progress = new UserProgress
                {
                    UserId = userId,
                    CourseId = courseId,
                    LectureId = lectureId,
                    QuizId = quizId,
                    QuestionId = questionId,
                    IsCompleted = true
                };
                _context.UserProgress.Add(progress);
            }
            else
            {
                progress.IsCompleted = true;
                _context.UserProgress.Update(progress);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserProgress>> GetUserProgress(string userId, int courseId)
        {
            return await _context.UserProgress
                .Where(p => p.UserId == userId && p.CourseId == courseId)
                .ToListAsync();
        }
    }
}
