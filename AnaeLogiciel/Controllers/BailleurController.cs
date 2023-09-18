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
    public class BailleurController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BailleurController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bailleur
        public async Task<IActionResult> Index()
        {
              return _context.Bailleur != null ? 
                          View(await _context.Bailleur.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Bailleur'  is null.");
        }

        // GET: Bailleur/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bailleur == null)
            {
                return NotFound();
            }

            var bailleur = await _context.Bailleur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bailleur == null)
            {
                return NotFound();
            }

            return View(bailleur);
        }

        // GET: Bailleur/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bailleur/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom")] Bailleur bailleur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bailleur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bailleur);
        }

        // GET: Bailleur/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bailleur == null)
            {
                return NotFound();
            }

            var bailleur = await _context.Bailleur.FindAsync(id);
            if (bailleur == null)
            {
                return NotFound();
            }
            return View(bailleur);
        }

        // POST: Bailleur/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] Bailleur bailleur)
        {
            if (id != bailleur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bailleur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BailleurExists(bailleur.Id))
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
            return View(bailleur);
        }

        // GET: Bailleur/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bailleur == null)
            {
                return NotFound();
            }

            var bailleur = await _context.Bailleur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bailleur == null)
            {
                return NotFound();
            }

            return View(bailleur);
        }

        // POST: Bailleur/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bailleur == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bailleur'  is null.");
            }
            var bailleur = await _context.Bailleur.FindAsync(id);
            if (bailleur != null)
            {
                _context.Bailleur.Remove(bailleur);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BailleurExists(int id)
        {
          return (_context.Bailleur?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
