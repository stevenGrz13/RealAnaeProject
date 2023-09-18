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
    public class SousActiviteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SousActiviteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SousActivite
        public async Task<IActionResult> Index()
        {
              return _context.SousActivite != null ? 
                          View(await _context.SousActivite.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.SousActivite'  is null.");
        }

        // GET: SousActivite/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SousActivite == null)
            {
                return NotFound();
            }

            var sousActivite = await _context.SousActivite
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sousActivite == null)
            {
                return NotFound();
            }

            return View(sousActivite);
        }

        // GET: SousActivite/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SousActivite/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom")] SousActivite sousActivite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sousActivite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sousActivite);
        }

        // GET: SousActivite/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SousActivite == null)
            {
                return NotFound();
            }

            var sousActivite = await _context.SousActivite.FindAsync(id);
            if (sousActivite == null)
            {
                return NotFound();
            }
            return View(sousActivite);
        }

        // POST: SousActivite/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] SousActivite sousActivite)
        {
            if (id != sousActivite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sousActivite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SousActiviteExists(sousActivite.Id))
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
            return View(sousActivite);
        }

        // GET: SousActivite/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SousActivite == null)
            {
                return NotFound();
            }

            var sousActivite = await _context.SousActivite
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sousActivite == null)
            {
                return NotFound();
            }

            return View(sousActivite);
        }

        // POST: SousActivite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SousActivite == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SousActivite'  is null.");
            }
            var sousActivite = await _context.SousActivite.FindAsync(id);
            if (sousActivite != null)
            {
                _context.SousActivite.Remove(sousActivite);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SousActiviteExists(int id)
        {
          return (_context.SousActivite?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
