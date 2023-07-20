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
    public class CourseController : Controller
    {
        private readonly ArtisanELearningSystemContext _context;

        public CourseController(ArtisanELearningSystemContext context)
        {
            _context = context;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            var artisanELearningSystemContext = _context.Course.Include(c => c.Curriculum).Include(c => c.Instructor);
            return View(await artisanELearningSystemContext.ToListAsync());
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Curriculum)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            ViewData["CurriculumId"] = new SelectList(_context.Set<Curriculum>(), "Id", "Id");
            ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "Id");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,LearningOutcomes,Requirements,Level,Category,CurriculumId,Price,Discount,IsLoginRequired,IsPublished,InstructorId,DateCreated")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurriculumId"] = new SelectList(_context.Set<Curriculum>(), "Id", "Id", course.CurriculumId);
            ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "Id", course.InstructorId);
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["CurriculumId"] = new SelectList(_context.Set<Curriculum>(), "Id", "Id", course.CurriculumId);
            ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "Id", course.InstructorId);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,LearningOutcomes,Requirements,Level,Category,CurriculumId,Price,Discount,IsLoginRequired,IsPublished,InstructorId,DateCreated")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            ViewData["CurriculumId"] = new SelectList(_context.Set<Curriculum>(), "Id", "Id", course.CurriculumId);
            ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "Id", course.InstructorId);
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Curriculum)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ArtisanELearningSystemContext.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
