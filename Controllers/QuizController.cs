using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArtisanELearningSystem.Data;
using ArtisanELearningSystem.Entities;
using ArtisanELearningSystem.Services.Interfaces;

namespace ArtisanELearningSystem.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // GET: Quiz/{quizId}
        public async Task<IActionResult> Details(int quizId)
        {
            var quiz = await _quizService.GetQuizByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // GET: Course/{courseId}/AddQuiz
        public IActionResult AddQuiz(int courseId)
        {
            var newQuiz = new Quiz { CourseId = courseId };
            return View(newQuiz);
        }

        // POST: Course/{courseId}/AddQuiz
        [HttpPost]
        public async Task<IActionResult> AddQuiz(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                await _quizService.CreateQuizAsync(quiz);
                return RedirectToAction("Details", "Course", new { id = quiz.CourseId });
            }
            return View(quiz);
        }

        // GET: Quiz/Edit/{quizId}
        public async Task<IActionResult> Edit(int quizId)
        {
            var quiz = await _quizService.GetQuizByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quiz/Edit/{quizId}
        [HttpPost]
        public async Task<IActionResult> Edit(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                await _quizService.UpdateQuizAsync(quiz);
                return RedirectToAction("Details", new { quizId = quiz.Id });
            }
            return View(quiz);
        }

        // POST: Quiz/Delete/{quizId}
        [HttpPost]
        public async Task<IActionResult> Delete(string quizId)
        {
            await _quizService.DeleteQuizAsync(quizId);
            return RedirectToAction("Index", "Home"); // Redirect to the home page or any other appropriate page
        }
    }
}