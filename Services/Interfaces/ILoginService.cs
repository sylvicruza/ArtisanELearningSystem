using ArtisanELearningSystem.Models;
using System.Security.Claims;

namespace ArtisanELearningSystem.Services.Interfaces
{
    public interface ILoginService
    {
        Task<ClaimsPrincipal> Authenticate(SignInViewModel model);
        Task<T> GetLoggedInUser<T>(string email) where T : class;
        Task<object> GetUserByEmailAsync(string email);
    }
}
