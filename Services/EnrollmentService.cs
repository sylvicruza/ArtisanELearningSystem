using ArtisanELearningSystem.Data;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Exceptions;
using ArtisanELearningSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtisanELearningSystem.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ArtisanELearningSystemContext _context;
        


        public EnrollmentService(ArtisanELearningSystemContext context)
        {
            _context = context;
           

        }
        public async Task EnrollStudentInCourse(int studentId, int courseId)
        {
            // Check if the student is already enrolled in the course
            if (_context.CourseEnrollment.Any(e => e.StudentId == studentId && e.CourseId == courseId))
            {
                throw new EnrollmentException("Oops!! You are already enrolled in this course try another course.");
            }

            // Check if the course is full (if applicable)
           /* var course = await _context.Course.FindAsync(courseId);
            if (course.IsFull)
            {
                throw new EnrollmentException("The course is full and no more enrollments are allowed.");
            }*/

            // Create a new CourseEnrollment record for the student and course
            var enrollment = new CourseEnrollment { StudentId = studentId, CourseId = courseId };
            _context.CourseEnrollment.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetEnrolledCoursesForStudent(int studentId)
        {
            var enrolledCourses = await _context.CourseEnrollment
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Instructor)
                .ToListAsync();

            return enrolledCourses.Select(e => e.Course).ToList();
        }

        public async Task<ICollection<CourseEnrollment>> GetEnrollmentByUser(Student user)
        {
            var enrolledCourses = await _context.CourseEnrollment
                .Where(e => e.StudentId == user.Id)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Instructor)
                .ToListAsync();

            return enrolledCourses;
        }

        public async Task<int> GetCourseEnrollmentByCourseId(int courseId)
        {
            return await _context.CourseEnrollment.CountAsync(e => e.CourseId == courseId);
        }



    }
}
