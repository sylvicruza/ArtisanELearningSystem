using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Models;

namespace ArtisanELearningSystem.Services.Interfaces
{
    public interface IStudentService
    {
        Task<bool> Authenticate(SignInViewModel model);
        Task<bool> CreateStudent(Student student);
        Task DeleteStudent(int id);
        Task<List<Student>> GetAllStudents();
        Task<Student> GetStudent(int? id);
        Task<Student> GetStudentByEmail(string email);
        Task<Student> GetStudentById(int? id);
        Task<Student> GetStudentByStudentId(string? studentId);
        bool StudentExists(int id);
        Task UpdateStudent(Student student);
    }
}
