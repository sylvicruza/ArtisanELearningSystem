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
    public class CourseRatingController : Controller
    {
        private readonly ArtisanELearningSystemContext _context;

        public CourseRatingController(ArtisanELearningSystemContext context)
        {
            _context = context;
        }

        // GET: CourseRating
        public async Task<IActionResult> Index()
        {
            var artisanELearningSystemContext = _context.CourseRating.Include(c => c.Course).Include(c => c.Student);
            return View(await artisanELearningSystemContext.ToListAsync());
        }

        // GET: CourseRating/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CourseRating == null)
            {
                return NotFound();
            }

            var courseRating = await _context.CourseRating
                .Include(c => c.Course)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseRating == null)
            {
                return NotFound();
            }

            return View(courseRating);
        }

        // GET: CourseRating/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id");
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id");
            return View();
        }

        // POST: CourseRating/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,StudentId,Rating,Comment,DateCreated,TimeAgo")] CourseRating courseRating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", courseRating.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id", courseRating.StudentId);
            return View(courseRating);
        }

        // GET: CourseRating/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CourseRating == null)
            {
                return NotFound();
            }

            var courseRating = await _context.CourseRating.FindAsync(id);
            if (courseRating == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", courseRating.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id", courseRating.StudentId);
            return View(courseRating);
        }

        // POST: CourseRating/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,StudentId,Rating,Comment,DateCreated,TimeAgo")] CourseRating courseRating)
        {
            if (id != courseRating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseRatingExists(courseRating.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", courseRating.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id", courseRating.StudentId);
            return View(courseRating);
        }

        // GET: CourseRating/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CourseRating == null)
            {
                return NotFound();
            }

            var courseRating = await _context.CourseRating
                .Include(c => c.Course)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseRating == null)
            {
                return NotFound();
            }

            return View(courseRating);
        }

        // POST: CourseRating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CourseRating == null)
            {
                return Problem("Entity set 'ArtisanELearningSystemContext.CourseRating'  is null.");
            }
            var courseRating = await _context.CourseRating.FindAsync(id);
            if (courseRating != null)
            {
                _context.CourseRating.Remove(courseRating);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseRatingExists(int id)
        {
          return (_context.CourseRating?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
