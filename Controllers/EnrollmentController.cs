using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArtisanELearningSystem.Data;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Exceptions;
using ArtisanELearningSystem.Services;
using ArtisanELearningSystem.Services.Interfaces;

namespace ArtisanELearningSystem.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ICourseService _courseService;
        private readonly ILoginService _loginService;

        public EnrollmentController(IEnrollmentService enrollmentService, ICourseService courseService,ILoginService loginService)
        {
            _enrollmentService = enrollmentService;
            _courseService = courseService;
            _loginService = loginService;
        }

        private int GetCurrentUserId()
        {
            var email = User?.Identity?.Name;
            var currentUserId = _loginService.GetLoggedInUser<Student>(email).Result.Id;
            return currentUserId;
        }

        // Action to display available courses for enrollment
        public async Task<IActionResult> AvailableCourses()
        {         
            var currentUserId = GetCurrentUserId();
            // Get courses not yet enrolled by the current student
            var availableCourses = await _courseService.GetAvailableCoursesForStudent(currentUserId);
            return View(availableCourses);
        }

        // Action to enroll in a course
        [HttpPost]
        public async Task<IActionResult> Enroll(int courseId)
        {           
            var currentUserId = GetCurrentUserId();
            try
            {
                await _enrollmentService.EnrollStudentInCourse(currentUserId, courseId);
                var successMessage = "User enrollment successful ";
                return RedirectToAction("Dashboard","Student", new { response = successMessage, type = "success" });
            }
            catch (EnrollmentException ex)
            {
                // Handle the EnrollmentException (e.g., course full, already enrolled) and display an error message
                ViewBag.Failure = ex.Message;
                return RedirectToAction("Dashboard", "Student", new { response = ex.Message, type = "failure" });
            }
        }

        // Action to display enrolled courses for the current student
        public async Task<IActionResult> EnrolledCourses()
        {
            var currentUserId = GetCurrentUserId();
            // Get enrolled courses for the current student
            var enrolledCourses = await _enrollmentService.GetEnrolledCoursesForStudent(currentUserId);
         //   ViewData[nameof(EnrolledCourses)] = enrolledCourses;
            return View(enrolledCourses);
        }
    }
}
