using ArtisanELearningSystem.Entities;

namespace ArtisanELearningSystem.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task EnrollStudentInCourse(int studentId, int courseId);
        Task<int> GetCourseEnrollmentByCourseId(int courseId);
        Task<List<Course>> GetEnrolledCoursesForStudent(int studentId);
        Task<ICollection<CourseEnrollment>> GetEnrollmentByUser(Student user);
    }
}
