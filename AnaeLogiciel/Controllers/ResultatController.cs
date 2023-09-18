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
    public class ResultatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Resultat
        public async Task<IActionResult> Index()
        {
              return _context.Resultat != null ? 
                          View(await _context.Resultat.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Resultat'  is null.");
        }

        // GET: Resultat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Resultat == null)
            {
                return NotFound();
            }

            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultat == null)
            {
                return NotFound();
            }

            return View(resultat);
        }

        // GET: Resultat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Resultat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom")] Resultat resultat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resultat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resultat);
        }

        // GET: Resultat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Resultat == null)
            {
                return NotFound();
            }

            var resultat = await _context.Resultat.FindAsync(id);
            if (resultat == null)
            {
                return NotFound();
            }
            return View(resultat);
        }

        // POST: Resultat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] Resultat resultat)
        {
            if (id != resultat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resultat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultatExists(resultat.Id))
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
            return View(resultat);
        }

        // GET: Resultat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Resultat == null)
            {
                return NotFound();
            }

            var resultat = await _context.Resultat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resultat == null)
            {
                return NotFound();
            }

            return View(resultat);
        }

        // POST: Resultat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Resultat == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Resultat'  is null.");
            }
            var resultat = await _context.Resultat.FindAsync(id);
            if (resultat != null)
            {
                _context.Resultat.Remove(resultat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultatExists(int id)
        {
          return (_context.Resultat?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult VersOccurenceResultat(int idprojet)
        {
            List<Resultat> liste = _context.Resultat
                .ToList();
            ViewBag.idprojet = idprojet;
            ViewData["listeresultat"] = liste;
            return View("~/Views/OccurenceResultat/Create.cshtml");
        }

        public IActionResult InsertionOccurenceResultat(int idprojet, List<int> idresultat)
        {
            foreach (var v in idresultat)
            {
                OccurenceResultat or = new OccurenceResultat()
                {
                    IdProjet = idprojet,
                    IdResultat = v
                };
                _context.Add(or);
            }

            _context.SaveChanges();
            ViewData["listeprojet"] = _context.Projet
                .Include(a => a.Bailleur)
                .ToList();
            return View("~/Views/Projet/Index.cshtml");
        }
    }
}
