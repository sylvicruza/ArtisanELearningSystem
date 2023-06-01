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
    public class StudentService : IStudentService
    {
        private readonly ArtisanELearningSystemContext _context;
       

        public StudentService(ArtisanELearningSystemContext context)
        {
            _context = context;
         
        }
        public async Task<bool> Authenticate(SignInViewModel model)
        {
            // Retrieve the student from the database using the provided username and password
            var student = await _context.Student.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (student == null)
            {
                return false;
            }
            var passwordHasher = new PasswordHasher<Student>();
            var result = passwordHasher.VerifyHashedPassword(student, student.Password, model.Password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<Student> GetStudentByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new UserNotFoundException($"Student with email {email} not found.");
            }
            var user = await _context.Student
               
                .FirstOrDefaultAsync(m => m.Email == email);
            return user;
        }

      

        public async Task<List<Student>> GetAllStudents() => await _context.Student.ToListAsync();

        public async Task<Student> GetStudentById(int? id)
        {
            if (id == null || _context.Student == null)
            {
                throw new UserNotFoundException($"Student with id {id} not found.");
            }

            var user = await _context.Student
                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                throw new UserNotFoundException($"Student with id {id} not found.");
            }


            return user;
        }


        public async Task<Student> GetStudentByStudentId(string? studentId)
        {
            if (studentId == null || _context.Student == null)
            {
                throw new UserNotFoundException($"Student with id {studentId} not found.");
            }

            var user = await _context.Student
                
                .FirstOrDefaultAsync(m => m.StudentId == studentId);
            if (user == null)
            {
                throw new UserNotFoundException($"Student with studentId {studentId} not found.");
            }


            return user;
        }

        public async Task<bool> CreateStudent(Student student)
        {
            var existingStudent = await _context.Student.FirstOrDefaultAsync(i => i.Email == student.Email);

            if (existingStudent != null)
            {
                // Email already exists, return false or throw an exception indicating the failure
                return false;
            }

            var passwordHasher = new PasswordHasher<Student>();
           
            var hashedPassword = passwordHasher.HashPassword(null,student.Password);
            student.Password = hashedPassword;
            student.StudentId = GenerateStudentId();
            _context.Add(student);
            await _context.SaveChangesAsync();

            return true; //Student created successfully
        }

        private string GenerateStudentId()
        {
            string studentId;
            var count = _context.Student.Count();
            var Today = DateTime.Now;
            var prefix = "STU";
            //This code generate student Number by Id
            switch (count)
            {
                case 0:
                    studentId = prefix + Today.Year.ToString() + 1.ToString("D4");

                    break;
                default:
                    int lastColumn = _context.Student.OrderBy(x => x.Id).LastOrDefault().Id;
                    lastColumn++;
                    studentId = prefix + Today.Year.ToString() + lastColumn.ToString("D4");

                    break;
            }
            return studentId;
        }

        
        public async Task UpdateStudent(Student student)
        {
            _context.Update(student);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteStudent(int id)
        {
            if (_context.Student == null)
            {
                throw new Exception("Entity set 'StudentFeedbackSystemContext.Student'  is null.");
            }
            var user = await _context.Student.FindAsync(id);
            if (user != null)
            {
                _context.Student.Remove(user);
            }

            await _context.SaveChangesAsync();

        }
        public async Task<Student> GetStudent(int? id)
        {
            if (id == null || _context.Student == null)
            {
                throw new UserNotFoundException($"Student with id {id} not found.");
            }

            var user = await _context.Student.FindAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException($"Student with id {id} not found.");
            }
            return user;
        }

        public bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
