using ArtisanELearningSystem.Models;
using ArtisanELearningSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ArtisanELearningSystem.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static ArtisanELearningSystem.Models.ForgotPasswordModel;

namespace ArtisanELearningSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginService _loginService;
        private readonly ICourseService _courseService;

        public HomeController(ILogger<HomeController> logger, ILoginService loginService, ICourseService courseService)
        {
            _logger = logger;
            _loginService = loginService;
            _courseService = courseService;
        }

        public IActionResult Index(string? userInfo)
        {
            if (!string.IsNullOrEmpty(userInfo))
            {
                ModelState.AddModelError(string.Empty, userInfo);
            }
            var courses = _courseService.GetAllCourses().Result;
            return View(courses);
        }

        public IActionResult SignIn(string? userInfo)
        {
            if (!string.IsNullOrEmpty(userInfo))
            {
                ModelState.AddModelError(string.Empty, userInfo);
            }



            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var message = "*Some fields missing";
                return RedirectToAction("SignIn", "Home", new { userInfo = message });
            }
            var principal = await _loginService.Authenticate(model);

            if (principal != null)
            {
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                    IsPersistent = model.RememberMe
                };

                // Authenticate the user and create an authentication cookie
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal, authProperties);




                // Redirect to the home page or to the URL the user originally requested
                return redirectToDashboard(principal);
            }
            else
            {
                // If the user's credentials are not valid, display an error message
                var message = "Invalid email or password";
                return RedirectToAction("SignIn", "Home", new { userInfo = message });

            }

        }

        public RedirectToActionResult redirectToDashboard(ClaimsPrincipal principal)
        {
            // Retrieve the role claim from the authenticated user's ClaimsPrincipal
            var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (roleClaim == null)
            {
                var message = "Error retrieving the role claim";
                return RedirectToAction("SignIn", "Home", new { userInfo = message });
            }

            if (roleClaim.Value.Contains(LoginService.Student))
            {
                return RedirectToAction("Dashboard", "Student");
            }
            else if (roleClaim.Value.Contains(LoginService.Instructor))
            {
                return RedirectToAction("Dashboard", "Instructor");
            }
            else
            {
                var message = "Role not found";
                return RedirectToAction("SignIn", "Home", new { userInfo = message });
            }


        }

        public IActionResult SignUp(string? response, string? type)
        {
            if (type == "success" && !string.IsNullOrEmpty(response))
            {
                ViewBag.Message = response;
            }
            else if (type == "failure" && !string.IsNullOrEmpty(response))
            {
                ViewBag.Failure = response;
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // Sign the user out and delete the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the home page or to the URL the user originally requested
            return RedirectToAction("SignIn", "Home");
        }


        public IActionResult ForgotPassword()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();
            model.Status = ForgotPasswordStatus.None;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {

            if (string.IsNullOrWhiteSpace(model.Input.Email))
            {
                var message = "*Email fields missing";
                return RedirectToAction("ForgotPassword", "Login", new { userInfo = message });
            }

            var user = await _loginService.GetUserByEmailAsync(model.Input.Email.Trim());
            var userObject = (dynamic)user;
            if (userObject.Status == ForgotPasswordStatus.Error)
            {
                model.Status = userObject.Status;
                model.ErrorMessage = userObject.ErrorMessage;
                return View(model);
            }
            else if (userObject.Status == ForgotPasswordStatus.Success)
            {
                model.Status = userObject.Status;
            }

            return View(model);
        }


       
        public IActionResult Settings()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}