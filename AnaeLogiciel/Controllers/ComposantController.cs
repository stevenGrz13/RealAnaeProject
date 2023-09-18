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
    public class ComposantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComposantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Composant
        public async Task<IActionResult> Index()
        {
              return _context.Composant != null ? 
                          View(await _context.Composant.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Composant'  is null.");
        }

        // GET: Composant/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Composant == null)
            {
                return NotFound();
            }

            var composant = await _context.Composant
                .FirstOrDefaultAsync(m => m.Id == id);
            if (composant == null)
            {
                return NotFound();
            }

            return View(composant);
        }

        // GET: Composant/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Composant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom")] Composant composant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(composant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(composant);
        }

        // GET: Composant/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Composant == null)
            {
                return NotFound();
            }

            var composant = await _context.Composant.FindAsync(id);
            if (composant == null)
            {
                return NotFound();
            }
            return View(composant);
        }

        // POST: Composant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] Composant composant)
        {
            if (id != composant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(composant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComposantExists(composant.Id))
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
            return View(composant);
        }

        // GET: Composant/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Composant == null)
            {
                return NotFound();
            }

            var composant = await _context.Composant
                .FirstOrDefaultAsync(m => m.Id == id);
            if (composant == null)
            {
                return NotFound();
            }

            return View(composant);
        }

        // POST: Composant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Composant == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Composant'  is null.");
            }
            var composant = await _context.Composant.FindAsync(id);
            if (composant != null)
            {
                _context.Composant.Remove(composant);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComposantExists(int id)
        {
          return (_context.Composant?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
