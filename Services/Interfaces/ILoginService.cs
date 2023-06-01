using ArtisanELearningSystem.Models;
using System.Security.Claims;

namespace ArtisanELearningSystem.Services.Interfaces
{
    public interface ILoginService
    {
        Task<ClaimsPrincipal> Authenticate(SignInViewModel model);
        Task<object> GetUserByEmailAsync(string email);
    }
}
