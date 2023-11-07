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
    public class ActeurController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActeurController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Acteur
        public async Task<IActionResult> Index()
        {
              return _context.Acteur != null ? 
                          View(await _context.Acteur.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Acteur'  is null.");
        }

        // GET: Acteur/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Acteur == null)
            {
                return NotFound();
            }

            var acteur = await _context.Acteur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acteur == null)
            {
                return NotFound();
            }

            return View(acteur);
        }

        // GET: Acteur/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Acteur/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom")] Acteur acteur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(acteur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(acteur);
        }

        // GET: Acteur/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Acteur == null)
            {
                return NotFound();
            }

            var acteur = await _context.Acteur.FindAsync(id);
            if (acteur == null)
            {
                return NotFound();
            }
            return View(acteur);
        }

        // POST: Acteur/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] Acteur acteur)
        {
            if (id != acteur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(acteur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActeurExists(acteur.Id))
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
            return View(acteur);
        }

        // GET: Acteur/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Acteur == null)
            {
                return NotFound();
            }

            var acteur = await _context.Acteur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acteur == null)
            {
                return NotFound();
            }

            return View(acteur);
        }

        // POST: Acteur/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Acteur == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Acteur'  is null.");
            }
            var acteur = await _context.Acteur.FindAsync(id);
            if (acteur != null)
            {
                _context.Acteur.Remove(acteur);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActeurExists(int id)
        {
          return (_context.Acteur?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
