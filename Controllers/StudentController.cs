using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Services.Interfaces;
using ArtisanELearningSystem.Models;
using ArtisanELearningSystem.Exceptions;
using ArtisanELearningSystem.Services;

namespace ArtisanELearningSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService _studentService)
        {
            studentService = _studentService;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
              return studentService.GetAllStudents() != null ? 
                          View(await studentService.GetAllStudents()) :
                          Problem("Entity set 'ArtisanELearningSystemContext.Student'  is null.");
        }

        // GET: Student/Dashboard
        public IActionResult Dashboard(string response)
        {
            if (!string.IsNullOrEmpty(response))
            {
                ViewBag.Message = response;
            }

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
        public IActionResult Courses()
        {
            return View();
        }
        // GET: Student/Messages
        public IActionResult Messages()
        {
            return View();
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
                    
                    AboutMe = signUp.AboutMe,
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
