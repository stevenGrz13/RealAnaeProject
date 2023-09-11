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
    public class ProjetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projet
        public async Task<IActionResult> Index()
        {
            List<Projet> projets = _context.Projet.ToList();
            foreach (var v in projets)
            {
                double avancement = 0;
                List<OccurenceActivite> listeoccurence = _context.ActiviteProjet
                    .Where(a => a.IdProjet == v.Id).ToList();
                foreach (var z in listeoccurence)
                {
                    avancement += z.Avancement;
                }

                double realavancement = avancement / listeoccurence.Count;

                if (Double.IsNaN(realavancement))
                {
                    v.Avancement = 0;
                }
                else
                {
                    v.Avancement = realavancement;   
                }

                _context.SaveChanges();
                
                if (v.Avancement>=100)
                {
                    v.FinishedOrNot = true;
                }
                else
                {
                    v.FinishedOrNot = false;
                }
                
                _context.SaveChanges();
            }
            return _context.Projet != null ? 
                          View(projets) :
                          Problem("Entity set 'ApplicationDbContext.Projet'  is null.");
        }

        // GET: Projet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projet == null)
            {
                return NotFound();
            }

            var projet = await _context.Projet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projet == null)
            {
                return NotFound();
            }

            return View(projet);
        }

        // GET: Projet/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,DateDebutPrevision,DateFinPrevision")] Projet projet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projet);
        }

        // GET: Projet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projet == null)
            {
                return NotFound();
            }

            var projet = await _context.Projet.FindAsync(id);
            if (projet == null)
            {
                return NotFound();
            }
            return View(projet);
        }

        // POST: Projet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,DateDebutPrevision,DateFinPrevision")] Projet projet)
        {
            if (id != projet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjetExists(projet.Id))
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
            return View(projet);
        }

        // GET: Projet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projet == null)
            {
                return NotFound();
            }

            var projet = await _context.Projet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projet == null)
            {
                return NotFound();
            }

            return View(projet);
        }

        // POST: Projet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projet'  is null.");
            }
            var projet = await _context.Projet.FindAsync(id);
            if (projet != null)
            {
                _context.Projet.Remove(projet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjetExists(int id)
        {
          return (_context.Projet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
