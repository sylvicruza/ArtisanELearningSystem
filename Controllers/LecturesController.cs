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
    public class LecturesController : Controller
    {
        private readonly ArtisanELearningSystemContext _context;

        public LecturesController(ArtisanELearningSystemContext context)
        {
            _context = context;
        }

        // GET: Lectures
        public async Task<IActionResult> Index()
        {
            var artisanELearningSystemContext = _context.Lecture.Include(l => l.Course);
            return View(await artisanELearningSystemContext.ToListAsync());
        }

        // GET: Lectures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lecture == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lecture
                .Include(l => l.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // GET: Lectures/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id");
            return View();
        }

        // POST: Lectures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsPreviewFree,VideoType,URL,Runtime,Attachment,CourseId")] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lecture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", lecture.CourseId);
            return View(lecture);
        }

        // GET: Lectures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lecture == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lecture.FindAsync(id);
            if (lecture == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", lecture.CourseId);
            return View(lecture);
        }

        // POST: Lectures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsPreviewFree,VideoType,URL,Runtime,Attachment,CourseId")] Lecture lecture)
        {
            if (id != lecture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectureExists(lecture.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", lecture.CourseId);
            return View(lecture);
        }

        // GET: Lectures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lecture == null)
            {
                return NotFound();
            }

            var lecture = await _context.Lecture
                .Include(l => l.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lecture == null)
            {
                return Problem("Entity set 'ArtisanELearningSystemContext.Lecture'  is null.");
            }
            var lecture = await _context.Lecture.FindAsync(id);
            if (lecture != null)
            {
                _context.Lecture.Remove(lecture);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LectureExists(int id)
        {
          return (_context.Lecture?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
