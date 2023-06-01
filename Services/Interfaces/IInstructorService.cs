using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Models;

namespace ArtisanELearningSystem.Services.Interfaces
{
    public interface IInstructorService
    {
        Task<bool> Authenticate(SignInViewModel model);
        Task<bool> CreateInstructor(Instructor Instructor);
        Task DeleteInstructor(int id);
        Task<List<Instructor>> GetAllInstructors();
        Task<Instructor> GetInstructor(int? id);
        Task<Instructor> GetInstructorByEmail(string email);
        Task<Instructor> GetInstructorById(int? id);
        bool InstructorExists(int id);
        Task UpdateInstructor(Instructor Instructor);
    }
}
