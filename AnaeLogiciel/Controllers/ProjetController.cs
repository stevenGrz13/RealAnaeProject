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
        public IActionResult Index()
        {
            ViewData["listeprojet"] = _context.Projet
                .Include(a => a.Bailleur)
                .ToList();
            return View();
        }

        // GET: Projet/Details/5
        public async Task<IActionResult> Details(int? idprojet)
        {
            if (idprojet == null || _context.Projet == null)
            {
                return NotFound();
            }

            var projet = await _context.Projet
                .Include(p => p.Bailleur)
                .FirstOrDefaultAsync(m => m.Id == idprojet);
            if (projet == null)
            {
                return NotFound();
            }

            ViewData["listeoccurenceresultat"] = _context.OccurenceResultat
                .Include(a => a.Resultat)
                .Where(a => a.IdProjet == idprojet).ToList();
            
            List<ProjetComposant> liste = _context.ProjetComposant
                .Include(a => a.Composant)
                .Where(a => a.IdProjet == idprojet).ToList();
            ViewData["listecomposant"] = liste;
            
            return View(projet);
        }

        // GET: Projet/Create
        public IActionResult Create()
        {
            ViewData["listebailleur"] = _context.Bailleur.ToList();
            ViewData["listecomposant"] = _context.Composant.ToList();
            return View();
        }

        public IActionResult CreateProjet(string nom, string details, DateOnly datedebut, DateOnly datefin, int idbailleur, List<int> idcomposant, string budget)
        {
            Projet projet = new Projet()
            {
                Nom = nom,
                Details = details,
                DateDebutPrevision = datedebut,
                DateFinPrevision = datefin,
                IdBailleur = idbailleur,
                Budget = Double.Parse(budget)
            };
            _context.Add(projet);
            _context.SaveChanges();
            Projet np = _context.Projet.First(a => a == projet);
            foreach (var v in idcomposant)
            {
                ProjetComposant pr = new ProjetComposant()
                {
                    IdProjet = np.Id,
                    IdComposant = v
                };
                _context.ProjetComposant.Add(pr);
            }
            
            _context.SaveChanges();
            ViewData["listeprojet"] = _context.Projet
                .Include(a => a.Bailleur)
                .ToList();
            return View("Index");
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
            ViewData["IdBailleur"] = new SelectList(_context.Bailleur, "Id", "Nom", projet.IdBailleur);
            return View(projet);
        }

        // POST: Projet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,DateDebutPrevision,DateFinPrevision,FinishedOrNot,Avancement,Details,IdBailleur")] Projet projet)
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
            ViewData["IdBailleur"] = new SelectList(_context.Bailleur, "Id", "Nom", projet.IdBailleur);
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
                .Include(p => p.Bailleur)
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
