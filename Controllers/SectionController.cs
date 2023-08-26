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
    public class SectionController : Controller
    {
        private readonly ArtisanELearningSystemContext _context;

        public SectionController(ArtisanELearningSystemContext context)
        {
            _context = context;
        }

        // GET: Section
        public async Task<IActionResult> Index()
        {
            var artisanELearningSystemContext = _context.Section.Include(s => s.Lecture);
            return View(await artisanELearningSystemContext.ToListAsync());
        }

        // GET: Section/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Section == null)
            {
                return NotFound();
            }

            var section = await _context.Section
                .Include(s => s.Lecture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // GET: Section/Create
        public IActionResult Create()
        {
            ViewData["LectureId"] = new SelectList(_context.Set<Lecture>(), "Id", "Id");
            return View();
        }

        // POST: Section/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LectureId,QuizId")] Section section)
        {
            if (ModelState.IsValid)
            {
                _context.Add(section);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LectureId"] = new SelectList(_context.Set<Lecture>(), "Id", "Id", section.LectureId);
            return View(section);
        }

        // GET: Section/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Section == null)
            {
                return NotFound();
            }

            var section = await _context.Section.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            ViewData["LectureId"] = new SelectList(_context.Set<Lecture>(), "Id", "Id", section.LectureId);
            return View(section);
        }

        // POST: Section/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LectureId,QuizId")] Section section)
        {
            if (id != section.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(section);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionExists(section.Id))
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
            ViewData["LectureId"] = new SelectList(_context.Set<Lecture>(), "Id", "Id", section.LectureId);
            return View(section);
        }

        // GET: Section/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Section == null)
            {
                return NotFound();
            }

            var section = await _context.Section
                .Include(s => s.Lecture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // POST: Section/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Section == null)
            {
                return Problem("Entity set 'ArtisanELearningSystemContext.Section'  is null.");
            }
            var section = await _context.Section.FindAsync(id);
            if (section != null)
            {
                _context.Section.Remove(section);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SectionExists(int id)
        {
          return (_context.Section?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
