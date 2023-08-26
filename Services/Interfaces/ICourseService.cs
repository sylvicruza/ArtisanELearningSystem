using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Models;

namespace ArtisanELearningSystem.Services.Interfaces
{
    public interface ICourseService
    {
        bool CourseExists(int id);
        Task<bool> CreateCourse(CreateCourseViewModel model);
        Task<bool> CreateLecture(LectureViewModel model);
        Task CreateSection(string sectionName);
        Task DeleteCourse(int id);
        Task<List<Course>> GetAllCourses();
        Task<List<Course>> GetAvailableCoursesForStudent(int studentId);
        Task<Course> GetCourse(int? id);
        Task<Course> GetCourseByCourseId(int? courseId);
        Task<IEnumerable<Course>> GetCourseByInstructorId(int instructorId);
        Task<Course> GetCourseId(int? id);
        Task<List<Course>> GetCoursesByCategory(string category);
        Task<IEnumerable<Lecture>> GetLectureByCourseId(int courseId);
        Task<IEnumerable<Lecture>> GetLectureByInstructorId(int instructorId);
        Task<List<Question>> GetQQuesttionsByInstructorId(int instructorId);
        Task<List<Quiz>> GetQuizByCourseId(int courseId);
        Task<List<Quiz>> GetQuizzesByInstructorId(int instructorId);
        Task<List<CourseRating>> GetRating(int CourseId);
        Task<bool> SubmitRating(CourseRating courseRating);
        Task UpdateCourse(Course course);
    }
}
