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
    public class PartenaireTechniqueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartenaireTechniqueController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PartenaireTechnique
        public async Task<IActionResult> Index()
        {
              return _context.PartenaireTechnique != null ? 
                          View(await _context.PartenaireTechnique.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.PartenaireTechnique'  is null.");
        }

        // GET: PartenaireTechnique/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PartenaireTechnique == null)
            {
                return NotFound();
            }

            var partenaireTechnique = await _context.PartenaireTechnique
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partenaireTechnique == null)
            {
                return NotFound();
            }

            return View(partenaireTechnique);
        }

        // GET: PartenaireTechnique/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PartenaireTechnique/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom")] PartenaireTechnique partenaireTechnique)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partenaireTechnique);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partenaireTechnique);
        }

        // GET: PartenaireTechnique/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PartenaireTechnique == null)
            {
                return NotFound();
            }

            var partenaireTechnique = await _context.PartenaireTechnique.FindAsync(id);
            if (partenaireTechnique == null)
            {
                return NotFound();
            }
            return View(partenaireTechnique);
        }

        // POST: PartenaireTechnique/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] PartenaireTechnique partenaireTechnique)
        {
            if (id != partenaireTechnique.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partenaireTechnique);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartenaireTechniqueExists(partenaireTechnique.Id))
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
            return View(partenaireTechnique);
        }

        // GET: PartenaireTechnique/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PartenaireTechnique == null)
            {
                return NotFound();
            }

            var partenaireTechnique = await _context.PartenaireTechnique
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partenaireTechnique == null)
            {
                return NotFound();
            }

            return View(partenaireTechnique);
        }

        // POST: PartenaireTechnique/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PartenaireTechnique == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PartenaireTechnique'  is null.");
            }
            var partenaireTechnique = await _context.PartenaireTechnique.FindAsync(id);
            if (partenaireTechnique != null)
            {
                _context.PartenaireTechnique.Remove(partenaireTechnique);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartenaireTechniqueExists(int id)
        {
          return (_context.PartenaireTechnique?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
