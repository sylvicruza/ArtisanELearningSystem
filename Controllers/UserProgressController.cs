using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArtisanELearningSystem.Data;
using ArtisanELearningSystem.Entities;

namespace ArtisanELearningSystem.Controllers
{
    public class UserProgressController : Controller
    {
        private readonly ArtisanELearningSystemContext _context;

        public UserProgressController(ArtisanELearningSystemContext context)
        {
            _context = context;
        }

        // GET: UserProgress
        public async Task<IActionResult> Index()
        {
            var artisanELearningSystemContext = _context.UserProgress.Include(u => u.Course).Include(u => u.Lecture).Include(u => u.Question);
            return View(await artisanELearningSystemContext.ToListAsync());
        }

        // GET: UserProgress/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserProgress == null)
            {
                return NotFound();
            }

            var userProgress = await _context.UserProgress
                .Include(u => u.Course)
                .Include(u => u.Lecture)
                .Include(u => u.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProgress == null)
            {
                return NotFound();
            }

            return View(userProgress);
        }

        // GET: UserProgress/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id");
            ViewData["LectureId"] = new SelectList(_context.Lecture, "Id", "Id");
            ViewData["QuestionId"] = new SelectList(_context.Set<Question>(), "Id", "Id");
            return View();
        }

        // POST: UserProgress/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CourseId,LectureId,QuizId,QuestionId,IsCompleted")] UserProgress userProgress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProgress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", userProgress.CourseId);
            ViewData["LectureId"] = new SelectList(_context.Lecture, "Id", "Id", userProgress.LectureId);
            ViewData["QuestionId"] = new SelectList(_context.Set<Question>(), "Id", "Id", userProgress.QuestionId);
            return View(userProgress);
        }

        // GET: UserProgress/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserProgress == null)
            {
                return NotFound();
            }

            var userProgress = await _context.UserProgress.FindAsync(id);
            if (userProgress == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", userProgress.CourseId);
            ViewData["LectureId"] = new SelectList(_context.Lecture, "Id", "Id", userProgress.LectureId);
            ViewData["QuestionId"] = new SelectList(_context.Set<Question>(), "Id", "Id", userProgress.QuestionId);
            return View(userProgress);
        }

        // POST: UserProgress/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CourseId,LectureId,QuizId,QuestionId,IsCompleted")] UserProgress userProgress)
        {
            if (id != userProgress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProgress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProgressExists(userProgress.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", userProgress.CourseId);
            ViewData["LectureId"] = new SelectList(_context.Lecture, "Id", "Id", userProgress.LectureId);
            ViewData["QuestionId"] = new SelectList(_context.Set<Question>(), "Id", "Id", userProgress.QuestionId);
            return View(userProgress);
        }

        // GET: UserProgress/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserProgress == null)
            {
                return NotFound();
            }

            var userProgress = await _context.UserProgress
                .Include(u => u.Course)
                .Include(u => u.Lecture)
                .Include(u => u.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProgress == null)
            {
                return NotFound();
            }

            return View(userProgress);
        }

        // POST: UserProgress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserProgress == null)
            {
                return Problem("Entity set 'ArtisanELearningSystemContext.UserProgress'  is null.");
            }
            var userProgress = await _context.UserProgress.FindAsync(id);
            if (userProgress != null)
            {
                _context.UserProgress.Remove(userProgress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProgressExists(int id)
        {
          return (_context.UserProgress?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
