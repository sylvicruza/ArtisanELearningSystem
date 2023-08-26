using ArtisanELearningSystem.Data;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Exceptions;
using ArtisanELearningSystem.Models;
using ArtisanELearningSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace ArtisanELearningSystem.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly ArtisanELearningSystemContext _context;

        public InstructorService(ArtisanELearningSystemContext context)
        {
            _context = context;
        }

        public async Task<bool> Authenticate(SignInViewModel model)
        {
            // Retrieve the instructor from the database using the provided username and password
            var instructor = await _context.Instructor.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (instructor == null)
            {
                return false;
            }
            var passwordHasher = new PasswordHasher<Instructor>();
            var result = passwordHasher.VerifyHashedPassword(instructor, instructor.Password, model.Password);
            return result == PasswordVerificationResult.Success;
        }


        public async Task<Instructor> GetInstructorByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return _context.Instructor.SingleOrDefault(u => u.Email == email);

        }
        public async Task<List<Instructor>> GetAllInstructors()
        {
            return await _context.Instructor.ToListAsync();
        }

        public async Task<Instructor> GetInstructorById(int? id)
        {
            if (id == null || _context.Instructor == null)
            {
                throw new UserNotFoundException($"Instructor with id {id} not found.");
            }

            var user = await _context.Instructor
                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                throw new UserNotFoundException($"Instructor with id {id} not found.");
            }


            return user;
        }

        public async Task<bool> CreateInstructor(Instructor instructor)
        {
            var existingInstructor = await _context.Instructor.FirstOrDefaultAsync(i => i.Email == instructor.Email);

            if (existingInstructor != null)
            {
                // Email already exists, return false or throw an exception indicating the failure
                return false;
            }

            var passwordHasher = new PasswordHasher<Instructor>();
            var hashedPassword = passwordHasher.HashPassword(null, instructor.Password);
            instructor.Password = hashedPassword;
            instructor.InstructorId = GenerateInstructorId();
            _context.Add(instructor);
            await _context.SaveChangesAsync();

            // Email the instructor with the welcome message (optional)
            // var emailService = new EmailSender("smtp.gmail.com", 587, "lemmings.group@gmail.com", "teamlemmings2$");
            // await emailService.SendEmailAsync(instructor.Email, "Welcome to MyApp", "Thank you for signing up for MyApp! Your password is: " + instructor.Password);

            return true; // Instructor created successfully
        }


        private string GenerateInstructorId()
        {
            string InstructorId;
            var count = _context.Instructor.Count();
            var Today = DateTime.Now;
            string initial = "INT";
            //This code generate Instructor Number by Id
            switch (count)
            {
                case 0:
                    InstructorId = initial + Today.Year.ToString() + 1.ToString("D4");

                    break;
                default:
                    int lastColumn = _context.Instructor.OrderBy(x => x.Id).LastOrDefault().Id;
                    lastColumn++;
                    InstructorId = initial + Today.Year.ToString() + lastColumn.ToString("D4");
                    break;
            }
            return InstructorId;
        }

      
        public async Task UpdateInstructor(Instructor Instructor)
        {
            _context.Update(Instructor);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteInstructor(int id)
        {
            if (_context.Instructor == null)
            {
                throw new Exception("Entity set 'InstructorFeedbackSystemContext.Instructor'  is null.");
            }
            var user = await _context.Instructor.FindAsync(id);
            if (user != null)
            {
                _context.Instructor.Remove(user);
            }

            await _context.SaveChangesAsync();

        }
        public async Task<Instructor> GetInstructor(int? id)
        {
            if (id == null || _context.Instructor == null)
            {
                throw new UserNotFoundException($"Instructor with id {id} not found.");
            }

            var user = await _context.Instructor.FindAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException($"Instructor with id {id} not found.");
            }
            return user;
        }

        public bool InstructorExists(int id) => _context.Instructor.Any(e => e.Id == id);
    }
}

