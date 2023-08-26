using ArtisanELearningSystem.Data;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Exceptions;
using ArtisanELearningSystem.Models;
using ArtisanELearningSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArtisanELearningSystem.Services
{
    public class CourseService : ICourseService
    {
        private readonly ArtisanELearningSystemContext _context;
        private readonly ILoginService _loginService;


        public CourseService(ArtisanELearningSystemContext context,ILoginService loginService)
        {
            _context = context;
            _loginService = loginService;   

        }

        public async Task<List<Course>> GetAllCourses()
        {
           var courses = await _context.Course.Include(c => c.Lectures).Include(c => c.Instructor).ToListAsync();
            return courses;

        }
        

        public async Task<Course> GetCourseId(int? id)
        {
            if (id == null || _context.Course == null)
            {
                throw new ObjectNotFoundException($"Course with id {id} not found.");
            }

            var course = await _context.Course
               
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                throw new ObjectNotFoundException($"Course with id {id} not found.");
            }
            return course;
        }

        public async Task<Course> GetCourseByCourseId(int? courseId)
        {
            if (courseId == null || _context.Course == null)
            {
                throw new ObjectNotFoundException($"Course with id {courseId} not found.");
            }

            var user = await _context.Course

                .FirstOrDefaultAsync(m => m.Id == courseId);
            if (user == null)
            {
                throw new ObjectNotFoundException($"Course with studentId {courseId} not found.");
            }


            return user;
        }

        public async Task<bool> CreateCourse(CreateCourseViewModel model)
        {
            var existingCourse = await _context.Course.FirstOrDefaultAsync(i => i.Title == model.Title);

            if (existingCourse != null)
            {
                // Course already exists, return false or throw an exception indicating the failure
                throw new DuplicateFoundException($"Course with title {model.Title} already exists");
            }

            var loggedInInstructor = await _loginService.GetLoggedInUser<Instructor>(model.CreatedBy);

            var course = new Course
            {
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                IsPublished = model.IsPublished,
                Discount = model.Discount,
                LearningOutcomes = model.LearningOutcomes,
                Price = model.Price,
                IsLoginRequired = model.IsLoginRequired,
                Requirements = model.Requirements,
                Level = model.Level,
                Tags = model.TagsString,
                DateCreated = DateTime.Now,
                Poster = model.Poster,
                Badge = model.Badge,
                Instructor = loggedInInstructor,
                InstructorId = loggedInInstructor.Id
            };

            _context.Add(course);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateLecture(LectureViewModel model)
        {
            var existingLecture = await _context.Lecture.FirstOrDefaultAsync(i => i.Title == model.Title);

            if (existingLecture != null)
            {
                throw new DuplicateFoundException($"Lecture with title {model.Title} already exists");
            }

            var lecture = new Lecture
            {
                Title = model.Title,
                Description = model.Description,
                IsPreviewFree = model.IsPreviewFree,
                VideoType = model.VideoType,
                URL = model.URL,
                Runtime = $"{model.hours}:{model.mins}:{model.secs}",
                Attachment = model.Poster,
                CourseId = model.CourseId,
                InstructorId = model.InstrustorId
            };

            _context.Add(lecture);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task UpdateCourse(Course course)
        {
            _context.Update(course);
            await _context.SaveChangesAsync();

        }


        public async Task<bool> SubmitRating(CourseRating courseRating)
        {
            
            _context.CourseRating.Add(courseRating);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CourseRating>> GetRating(int CourseId)
        {

return await _context.CourseRating
              .Include(c => c.Course)
              .Include(c => c.Student)
              .Where(m => m.CourseId == CourseId).ToListAsync();
        }


        public async Task DeleteCourse(int id)
        {
            if (_context.Course == null)
            {
                throw new Exception("Entity set 'StudentFeedbackSystemContext.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }

            await _context.SaveChangesAsync();

        }
        public async Task<Course> GetCourse(int? id)
        {
            if (id == null || _context.Course == null)
            {
                throw new ObjectNotFoundException($"Course with id {id} not found.");
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                throw new ObjectNotFoundException($"Course with id {id} not found.");
            }
            return course;
        }

        public bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }

        public async Task CreateSection(string sectionName)
        {
            var existingSection = await _context.Section.FirstOrDefaultAsync(i => i.Name == sectionName);

            Console.WriteLine(existingSection);
            var section = new Section { Name = sectionName };
            _context.Section.Add(section);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> GetCourseByInstructorId(int instructorId)
        {
            var courses = await _context.Course.Where(m => m.InstructorId == instructorId).ToListAsync();

            if (!courses.Any())
            {
                throw new ObjectNotFoundException($"Course with instructorId {instructorId} not found.");
            }

            return courses;
        }
        public async Task<IEnumerable<Lecture>> GetLectureByInstructorId(int instructorId)
        {
            var lectures = await _context.Lecture.Include(x=>x.Course).Where(m => m.InstructorId == instructorId).ToListAsync();

            if (!lectures.Any())
            {
                throw new ObjectNotFoundException($"Lecture with instructorId {instructorId} not found.");
            }

            return lectures;
        }
        public async Task<IEnumerable<Lecture>> GetLectureByCourseId(int courseId)
        {
            var lectures = await _context.Lecture.Include(a=>a.Instructor).Where(m => m.CourseId == courseId).ToListAsync();

            if (!lectures.Any())
            {
                return new List<Lecture>();
            }

            return lectures;
        }


        public async Task<List<Quiz>> GetQuizByCourseId(int courseId)
        {
            var quiz = await _context.Quiz
                .Include(a => a.Course)
                .Include(a => a.Questions)
                    .ThenInclude(q => q.Options) // Include the Options for each Question
                .Where(m => m.CourseId == courseId)
                .ToListAsync();

            if (!quiz.Any())
            {
                return new List<Quiz>();
            }

            return quiz;
        }

        public async Task<List<Quiz>> GetQuizzesByInstructorId(int instructorId)
        {
            var quizzes = await _context.Quiz
                .Include(q => q.Course)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Options)
                .Where(q => q.Course.InstructorId == instructorId)
                .ToListAsync();

            return quizzes;
        }

        public async Task<List<Question>> GetQQuesttionsByInstructorId(int instructorId)
        {
            var questions = await _context.Question
                .Include(q => q.Quiz)
                .Include(q => q.Options)
                   
                .Where(q => q.Quiz.Course.InstructorId == instructorId)
                .ToListAsync();

            return questions;
        }


        public async Task<List<Course>> GetAvailableCoursesForStudent(int studentId)
        {
            // Get the courses that the student is not enrolled in
            var enrolledCourseIds = await _context.CourseEnrollment
                .Where(e => e.StudentId == studentId)
                .Select(e => e.CourseId)
                .ToListAsync();

            // Retrieve courses that the student is not enrolled in
            var availableCourses = await _context.Course
                .Where(c => !enrolledCourseIds.Contains(c.Id))
                .ToListAsync();

            return availableCourses;
        }

        private int ConvertDifficultyLevel(string level)
        {
            // Convert course difficulty level (e.g., "Beginner", "Intermediate", "Advanced") to a numerical value
            if (level == "Beginner") return 1;
            if (level == "Intermediate") return 2;
            if (level == "Advanced") return 3;
            return 0; // Default value for unknown levels
        }

        public async Task<List<Course>> GetCoursesByCategory(string category)
        {
            var courses = await _context.Course
                .Where(c => c.Category == category)
                .Include(c => c.Lectures)
                .Include(c => c.Instructor)
                .ToListAsync();

            return courses;
        }

    }
}
