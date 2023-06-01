
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Services.Interfaces;
using ArtisanELearningSystem.Services;
using ArtisanELearningSystem.Models;
using ArtisanELearningSystem.Exceptions;

namespace ArtisanELearningSystem.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IInstructorService instructorService;

        public InstructorController(IInstructorService _instructorService)
        {
            instructorService = _instructorService;
        }

        // GET: Instructor
        public async Task<IActionResult> Index()
        {
              return instructorService.GetAllInstructors()!= null ? 
                          View(await instructorService.GetAllInstructors()) :
                          Problem("Entity set 'ArtisanELearningSystemContext.Instructor'  is null.");
        }

        // GET: Instructor/Dashboard
        public IActionResult Dashboard()
        {

            return View();

        }

        private async Task<Instructor> GetCurrentUser()
        {
            var identity = User.Identity;
            if (identity == null || string.IsNullOrEmpty(identity.Name))
            {
                throw new UserNotFoundException("User not found");

            }
            var instructor = await instructorService.GetInstructorByEmail(identity.Name);
            if (instructor == null)
            {
                throw new UserNotFoundException("User not found");

            }
            return instructor;
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || instructorService.GetInstructor(id) == null)
            {
                return NotFound();
            }

            var instructor = await instructorService.GetInstructor(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }
        // GET: Instructor/Courses
        public IActionResult Courses()
        {
            return View();
        }
        // GET: Instructor/Analytics
        public IActionResult Analytics()
        {
            return View();
        }

        // GET: Instructor/NewCourses
        public IActionResult NewCourses()
        {
            return View();
        }
        // GET: Instructor/Messages
        public IActionResult Messages()
        {
            return View();
        }
        // GET: Instructor/Notifications
        public IActionResult Notifications()
        {
            return View();
        }
        // GET: Instructor/Reviews
        public IActionResult Reviews()
        {
            return View();
        }
        // GET: Instructor/Statements
        public IActionResult Statements()
        {
            return View();
        }

        // GET: Instructor/Profile
        public IActionResult Profile()
        {
            return View();
        }
        // GET: Instructor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SignUpViewModel signUp)
        {
            if (ModelState.IsValid)
            {
                var instructor = new Instructor
                {
                    Name = signUp.Name,
                    Email = signUp.Email,
                    Category = signUp.Category,
                    AboutMe = signUp.AboutMe,
                    Password = signUp.Password
                };

                bool instructorCreated = await instructorService.CreateInstructor(instructor);

                if (instructorCreated)
                {
                    var successMessage = "User created successfully";
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


        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || instructorService.GetInstructor(id) == null)
            {
                return NotFound();
            }

            var instructor = await instructorService.GetInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Address,InstructorId,Mobile,AboutMe,Id,Email,Password,Name")] Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await instructorService.UpdateInstructor(instructor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!instructorService.InstructorExists(instructor.Id))
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
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || instructorService.GetInstructor(id) == null)
            {
                return NotFound();
            }

            var instructor = await instructorService.GetInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await instructorService.DeleteInstructor(id);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
