using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Services;
using ArtisanELearningSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArtisanELearningSystem.Controllers
{
    public class RecommendationController : Controller
    {
        private readonly IRecommendationService recommendationService;
        private readonly ILoginService loginService;

        public RecommendationController(IRecommendationService recommendationService,ILoginService loginService)
        {
            this.recommendationService = recommendationService;
            this.loginService = loginService;
        }

        // Action to display personalized learning path suggestions for the user
        public IActionResult PersonalizedLearningPath()
        {
            var email = User?.Identity?.Name;
            var user = loginService.GetLoggedInUser<Student>(email).Result; // Get the user with the specified ID
            if (user == null)
            {
                return NotFound();
            }

            var numberOfSuggestions = 5; // You can set the number of learning path suggestions to display
            var recommendations = recommendationService.GetPersonalizedLearningPaths(user, numberOfSuggestions);

            return View(recommendations);
        }
    }
}
