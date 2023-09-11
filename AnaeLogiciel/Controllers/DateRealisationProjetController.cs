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
    public class DateRealisationProjetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DateRealisationProjetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DateRealisationProjet
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DateRealisationProjet.Include(d => d.Projet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DateRealisationProjet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DateRealisationProjet == null)
            {
                return NotFound();
            }

            var dateRealisationProjet = await _context.DateRealisationProjet
                .Include(d => d.Projet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dateRealisationProjet == null)
            {
                return NotFound();
            }

            return View(dateRealisationProjet);
        }

        // GET: DateRealisationProjet/Create
        public IActionResult Create()
        {
            ViewData["IdProjet"] = new SelectList(_context.Projet, "Id", "Id");
            return View();
        }

        // POST: DateRealisationProjet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdProjet,DateDebutRealisation,DateFinRealisation")] DateRealisationProjet dateRealisationProjet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dateRealisationProjet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProjet"] = new SelectList(_context.Projet, "Id", "Id", dateRealisationProjet.IdProjet);
            return View(dateRealisationProjet);
        }

        // GET: DateRealisationProjet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DateRealisationProjet == null)
            {
                return NotFound();
            }

            var dateRealisationProjet = await _context.DateRealisationProjet.FindAsync(id);
            if (dateRealisationProjet == null)
            {
                return NotFound();
            }
            ViewData["IdProjet"] = new SelectList(_context.Projet, "Id", "Id", dateRealisationProjet.IdProjet);
            return View(dateRealisationProjet);
        }

        // POST: DateRealisationProjet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdProjet,DateDebutRealisation,DateFinRealisation")] DateRealisationProjet dateRealisationProjet)
        {
            if (id != dateRealisationProjet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dateRealisationProjet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DateRealisationProjetExists(dateRealisationProjet.Id))
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
            ViewData["IdProjet"] = new SelectList(_context.Projet, "Id", "Id", dateRealisationProjet.IdProjet);
            return View(dateRealisationProjet);
        }

        // GET: DateRealisationProjet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DateRealisationProjet == null)
            {
                return NotFound();
            }

            var dateRealisationProjet = await _context.DateRealisationProjet
                .Include(d => d.Projet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dateRealisationProjet == null)
            {
                return NotFound();
            }

            return View(dateRealisationProjet);
        }

        // POST: DateRealisationProjet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DateRealisationProjet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DateRealisationProjet'  is null.");
            }
            var dateRealisationProjet = await _context.DateRealisationProjet.FindAsync(id);
            if (dateRealisationProjet != null)
            {
                _context.DateRealisationProjet.Remove(dateRealisationProjet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DateRealisationProjetExists(int id)
        {
          return (_context.DateRealisationProjet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
