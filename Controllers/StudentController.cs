using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Services.Interfaces;
using ArtisanELearningSystem.Models;
using ArtisanELearningSystem.Exceptions;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder;
using ArtisanELearningSystem.ChatBox;

namespace ArtisanELearningSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;
        private readonly ICourseService courseService;
        private readonly IEnrollmentService enrollmentService;
        private readonly IDiscussionService discussionService;
        private readonly IRecommendationService recommendationService;
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly IInstructorService instructorService;
      
     //   private readonly AIMLChatBot _chatBot;



        public StudentController(IStudentService _studentService, ICourseService courseService, IEnrollmentService enrollmentService, IDiscussionService discussionService, IRecommendationService recommendationService, IInstructorService instructorService)
        {
            studentService = _studentService;
            this.courseService = courseService;
            this.enrollmentService = enrollmentService;
            this.discussionService = discussionService;
            this.recommendationService = recommendationService;
            this.instructorService = instructorService;
            // Initialize the chatbot with a unique user ID
           // _chatBot = new AIMLChatBot("user123");
        }

        [HttpPost]
        public async Task<string> PostAsync(CancellationToken cancellationToken)
        {
            // Process the incoming activity using the bot
            // await _adapter.ProcessAsync(Request, Response, _bot, cancellationToken);
            return  "Hello and welcome! I am your chatbot. How can I assist you?";
        }

       
/*
        [HttpPost]
        public IActionResult PostChatOutput(string message)
        {
            // Get the chatbot's response based on user input
            string response = _chatBot.GetOutput(message);

            // You can do something with the response, such as displaying it to the user
            return Json(new { response });
        }*/

        public IActionResult ChatIndex()
        {
            return View();
        }


        // GET: Student
        public async Task<IActionResult> Index()
        {
            return studentService.GetAllStudents() != null ?
                        View(await studentService.GetAllStudents()) :
                        Problem("Entity set 'ArtisanELearningSystemContext.Student'  is null.");
        }

        // GET: Student/Dashboard
        public async Task<IActionResult> Dashboard(string? response, string? type)
        {
            if (type == "success" && !string.IsNullOrEmpty(response))
            {
                ViewBag.Message = response;
            }
            else if (type == "failure" && !string.IsNullOrEmpty(response))
            {
                ViewBag.Failure = response;
            }
            ViewData["Courses"] = courseService.GetAllCourses().Result;
            var enrolledCourses = await enrollmentService.GetEnrolledCoursesForStudent(GetCurrentUser().Result.Id);
            ViewData["EnrolledCourses"] = enrolledCourses;
            
            var numberOfSuggestions = 5; // You can set the number of learning path suggestions to display
            var recommendations = recommendationService.GetPersonalizedLearningPaths(GetCurrentUser().Result, numberOfSuggestions);
            ViewData["RecommendedCourses"] = recommendations;
            ViewData["InstructorCount"] = instructorService.GetAllInstructors().Result.Count;

            return View();

        }
        private async Task<Student> GetCurrentUser()
        {
            var identity = User.Identity;
            if (identity == null || string.IsNullOrEmpty(identity.Name))
            {
                throw new UserNotFoundException("User not found");

            }
            var student = await studentService.GetStudentByEmail(identity.Name);
            if (student == null)
            {
                throw new UserNotFoundException("User not found");

            }
            return student;
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || studentService.GetStudent(id) == null)
            {
                return NotFound();
            }

            var student = await studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Certificates
        public IActionResult Certificates()
        {
            return View();
        }

        // GET: Student/Courses
        public async Task<IActionResult> Courses()
        {
            var currentUserId = GetCurrentUser().Result.Id;
            // Get enrolled courses for the current student
            var enrolledCourses = await enrollmentService.GetEnrolledCoursesForStudent(currentUserId);
            return View(enrolledCourses);
        }

        // GET: Student/SavedCourses
        public IActionResult SavedCourses()
        {
            return View();
        }
        // GET: Student/Messages

        public async Task<IActionResult> Messages(string? response, int courseId = 6)
        {
            if (!string.IsNullOrEmpty(response))
            {
                ViewBag.Message = response;
            }
            var discussion = new CourseDiscussionViewModel { CourseId = courseId };
            var discussions = await discussionService.GetDiscussionsForCourse(courseId);
            ViewData["Discussions"] = discussions.ToList();
            ViewData["LoggedInUser"] = GetCurrentUser().Result;
            return View(discussion);
        }



        // GET: Student/Notifications
        public IActionResult Notifications()
        {
            return View();
        }

        // GET: Student/Reviews
        public IActionResult Reviews()
        {
            return View();
        }
        // GET: Student/Statements
        public IActionResult Statements()
        {
            return View();
        }

        // GET: Student/Profile
        public IActionResult Profile()
        {
            return View(GetCurrentUser().Result);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SignUpViewModel signUp)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    Name = signUp.Name,
                    Email = signUp.Email,
                    PreferredDifficultyLevel = signUp.PreferredDifficultyLevel,
                    LearningObjectives = signUp.LearningObjectivesString,
                    Interests = signUp.InterestsString,
                    Password = signUp.Password
                };


                bool studentCreated = await studentService.CreateStudent(student);
                if (studentCreated)
                {
                    var successMessage = "User created successfully ";
                    return RedirectToAction("SignUp", "Home", new { response = successMessage, type = "success" });
                }
                else
                {
                    var failureMessage = "User creation failed. Email already exists.";
                    return RedirectToAction("SignUp", "Home", new { response = failureMessage, type = "failure" });
                }
            }

            var validationErrorMessage = "Invalid form data. Please check your inputs.";
            return RedirectToAction("SignUp", "Home", new { response = validationErrorMessage, type = "failure" });
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || studentService.GetStudent(id) == null)
            {
                return NotFound();
            }

            var student = await studentService.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Address,StudentId,Mobile,AboutMe,Id,Email,Password,Name")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await studentService.UpdateStudent(student);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!studentService.StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || studentService.GetStudent(id) == null)
            {
                return NotFound();
            }

            var student = await studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            try
            {
                await studentService.DeleteStudent(id);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
