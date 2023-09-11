using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnaeLogiciel.Data;
using AnaeLogiciel.Models;

namespace AnaeLogiciel.Controllers
{
    public class SourceDeVerificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SourceDeVerificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SourceDeVerification
        public async Task<IActionResult> Index()
        {
              return _context.SourceDeVerification != null ? 
                          View(await _context.SourceDeVerification.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.SourceDeVerification'  is null.");
        }

        // GET: SourceDeVerification/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SourceDeVerification == null)
            {
                return NotFound();
            }

            var sourceDeVerification = await _context.SourceDeVerification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sourceDeVerification == null)
            {
                return NotFound();
            }

            return View(sourceDeVerification);
        }

        // GET: SourceDeVerification/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SourceDeVerification/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom")] SourceDeVerification sourceDeVerification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sourceDeVerification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sourceDeVerification);
        }

        // GET: SourceDeVerification/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SourceDeVerification == null)
            {
                return NotFound();
            }

            var sourceDeVerification = await _context.SourceDeVerification.FindAsync(id);
            if (sourceDeVerification == null)
            {
                return NotFound();
            }
            return View(sourceDeVerification);
        }

        // POST: SourceDeVerification/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] SourceDeVerification sourceDeVerification)
        {
            if (id != sourceDeVerification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sourceDeVerification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SourceDeVerificationExists(sourceDeVerification.Id))
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
            return View(sourceDeVerification);
        }

        // GET: SourceDeVerification/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SourceDeVerification == null)
            {
                return NotFound();
            }

            var sourceDeVerification = await _context.SourceDeVerification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sourceDeVerification == null)
            {
                return NotFound();
            }

            return View(sourceDeVerification);
        }

        // POST: SourceDeVerification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SourceDeVerification == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SourceDeVerification'  is null.");
            }
            var sourceDeVerification = await _context.SourceDeVerification.FindAsync(id);
            if (sourceDeVerification != null)
            {
                _context.SourceDeVerification.Remove(sourceDeVerification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SourceDeVerificationExists(int id)
        {
          return (_context.SourceDeVerification?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
