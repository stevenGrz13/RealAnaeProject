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
    public class TypeIndicateurController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeIndicateurController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypeIndicateur
        public async Task<IActionResult> Index()
        {
              return _context.TypeIndicateur != null ? 
                          View(await _context.TypeIndicateur.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.TypeIndicateur'  is null.");
        }

        // GET: TypeIndicateur/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TypeIndicateur == null)
            {
                return NotFound();
            }

            var typeIndicateur = await _context.TypeIndicateur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeIndicateur == null)
            {
                return NotFound();
            }

            return View(typeIndicateur);
        }

        // GET: TypeIndicateur/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeIndicateur/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom")] TypeIndicateur typeIndicateur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeIndicateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeIndicateur);
        }

        // GET: TypeIndicateur/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TypeIndicateur == null)
            {
                return NotFound();
            }

            var typeIndicateur = await _context.TypeIndicateur.FindAsync(id);
            if (typeIndicateur == null)
            {
                return NotFound();
            }
            return View(typeIndicateur);
        }

        // POST: TypeIndicateur/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] TypeIndicateur typeIndicateur)
        {
            if (id != typeIndicateur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeIndicateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeIndicateurExists(typeIndicateur.Id))
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
            return View(typeIndicateur);
        }

        // GET: TypeIndicateur/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TypeIndicateur == null)
            {
                return NotFound();
            }

            var typeIndicateur = await _context.TypeIndicateur
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeIndicateur == null)
            {
                return NotFound();
            }

            return View(typeIndicateur);
        }

        // POST: TypeIndicateur/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TypeIndicateur == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TypeIndicateur'  is null.");
            }
            var typeIndicateur = await _context.TypeIndicateur.FindAsync(id);
            if (typeIndicateur != null)
            {
                _context.TypeIndicateur.Remove(typeIndicateur);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeIndicateurExists(int id)
        {
          return (_context.TypeIndicateur?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
