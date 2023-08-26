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
    public class DiscussionController : Controller
    {
        private readonly ArtisanELearningSystemContext _context;

        public DiscussionController(ArtisanELearningSystemContext context)
        {
            _context = context;
        }

        // GET: Discussion
        public async Task<IActionResult> Index()
        {
            var artisanELearningSystemContext = _context.Discussion.Include(d => d.Course);
            return View(await artisanELearningSystemContext.ToListAsync());
        }

        // GET: Discussion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Discussion == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .Include(d => d.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // GET: Discussion/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id");
            return View();
        }

        // POST: Discussion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,UserId,Content,Timestamp")] Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", discussion.CourseId);
            return View(discussion);
        }

        // GET: Discussion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Discussion == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", discussion.CourseId);
            return View(discussion);
        }

        // POST: Discussion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,UserId,Content,Timestamp")] Discussion discussion)
        {
            if (id != discussion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", discussion.CourseId);
            return View(discussion);
        }

        // GET: Discussion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Discussion == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .Include(d => d.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Discussion == null)
            {
                return Problem("Entity set 'ArtisanELearningSystemContext.Discussion'  is null.");
            }
            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion != null)
            {
                _context.Discussion.Remove(discussion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
          return (_context.Discussion?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
