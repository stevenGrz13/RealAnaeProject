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
    public class PartiePrenanteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartiePrenanteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PartiePrenante
        public async Task<IActionResult> Index()
        {
              return _context.Partieprenante != null ? 
                          View(await _context.Partieprenante.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Partieprenante'  is null.");
        }

        // GET: PartiePrenante/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Partieprenante == null)
            {
                return NotFound();
            }

            var partieprenante = await _context.Partieprenante
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partieprenante == null)
            {
                return NotFound();
            }

            return View(partieprenante);
        }

        // GET: PartiePrenante/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PartiePrenante/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom")] PartiePrenante partiePrenante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partiePrenante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(partiePrenante);
        }

        // GET: PartiePrenante/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Partieprenante == null)
            {
                return NotFound();
            }

            var partieprenante = await _context.Partieprenante.FindAsync(id);
            if (partieprenante == null)
            {
                return NotFound();
            }
            return View(partieprenante);
        }

        // POST: PartiePrenante/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] PartiePrenante partiePrenante)
        {
            if (id != partiePrenante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partiePrenante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartieprenanteExists(partiePrenante.Id))
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
            return View(partiePrenante);
        }

        // GET: PartiePrenante/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Partieprenante == null)
            {
                return NotFound();
            }

            var partieprenante = await _context.Partieprenante
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partieprenante == null)
            {
                return NotFound();
            }

            return View(partieprenante);
        }

        // POST: PartiePrenante/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Partieprenante == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Partieprenante'  is null.");
            }
            var partieprenante = await _context.Partieprenante.FindAsync(id);
            if (partieprenante != null)
            {
                _context.Partieprenante.Remove(partieprenante);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartieprenanteExists(int id)
        {
          return (_context.Partieprenante?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
