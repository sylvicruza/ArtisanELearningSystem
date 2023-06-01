using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Models;
using ArtisanELearningSystem.Services.Interfaces;
using ArtisanELearningSystem.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static ArtisanELearningSystem.Models.ForgotPasswordModel;

namespace ArtisanELearningSystem.Services
{
    public class LoginService : ILoginService
    {
        public const string Student = "Student";
        public const string Instructor = "Instructor";

 
        private readonly IStudentService _studentService;
        private readonly IInstructorService _instructorService;


        public LoginService(IStudentService studentService, IInstructorService instructorService)
        {
            _studentService = studentService;
            _instructorService = instructorService;

        }

        public async Task<ClaimsPrincipal> Authenticate(SignInViewModel model)
        {
            var user = await GetRoleAndUser(model);

            if (user.Value)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Email),
            new Claim(ClaimTypes.Role, user.Key),
            // Add any other claims as needed
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                return principal;
            }
            else
            {
                return null;
            }
        }

        private async Task<KeyValuePair<string, bool>> GetRoleAndUser(SignInViewModel model)
        {
            bool isStudent =  await _studentService.Authenticate(model);
            bool isInstructor = await _instructorService.Authenticate(model);

            if (isStudent)
            {
                return new KeyValuePair<string, bool>(Student, true);
            }
            else if (isInstructor)
            {
                return new KeyValuePair<string, bool>(Instructor, true);
            }

            return new KeyValuePair<string, bool>(string.Empty, false);
        }



        public async Task<object> GetUserByEmailAsync(string email)
        {
            object user = await GetAllUserByEmailAsync(email);

            if (user == null)
            {
                return new { Status = ForgotPasswordStatus.Error, ErrorMessage = "User not found." };
            }

            var newPassword = GeneratePassword(10);
            var hashedPassword = HashPassword(newPassword);

            switch (user)
            {


                case Student student:
                    student.Password = hashedPassword;
                    await _studentService.UpdateStudent(student);
                    break;

                case Instructor instructor:
                    instructor.Password = hashedPassword;
                    await _instructorService.UpdateInstructor(instructor);
                    break;

                default:
                    return new { Status = ForgotPasswordStatus.Error, ErrorMessage = "User not found." };
            }

            // await SendPasswordResetEmailAsync(email, newPassword);

            return new { Status = ForgotPasswordStatus.Success };
        }

        private async Task<object> GetAllUserByEmailAsync(string email)
        {

            object user = await _studentService.GetStudentByEmail(email);

            if (user != null)
            {
                return user;
            }

            return await _instructorService.GetInstructorByEmail(email);
        }

        private string GeneratePassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }

        private async Task SendPasswordResetEmailAsync(string email, string newPassword)
        {
            var emailSender = new EmailSender("localhost", 25, "example@mail.com", "password123");
            var emailBody = $"Your new password is: {newPassword}";
            await emailSender.SendEmailAsync(email, "Password Reset", emailBody);
        }



    }
}
