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
    public class TechnicienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TechnicienController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Technicien
        public async Task<IActionResult> Index()
        {
              return _context.Technicien != null ? 
                          View(await _context.Technicien.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Technicien'  is null.");
        }

        // GET: Technicien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Technicien == null)
            {
                return NotFound();
            }

            var technicien = await _context.Technicien
                .FirstOrDefaultAsync(m => m.Id == id);
            if (technicien == null)
            {
                return NotFound();
            }

            return View(technicien);
        }

        // GET: Technicien/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Technicien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Pass,Token,isAffected")] Technicien technicien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(technicien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(technicien);
        }

        // GET: Technicien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Technicien == null)
            {
                return NotFound();
            }

            var technicien = await _context.Technicien.FindAsync(id);
            if (technicien == null)
            {
                return NotFound();
            }
            return View(technicien);
        }

        // POST: Technicien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Pass,Token,isAffected")] Technicien technicien)
        {
            if (id != technicien.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(technicien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechnicienExists(technicien.Id))
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
            return View(technicien);
        }

        // GET: Technicien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Technicien == null)
            {
                return NotFound();
            }

            var technicien = await _context.Technicien
                .FirstOrDefaultAsync(m => m.Id == id);
            if (technicien == null)
            {
                return NotFound();
            }

            return View(technicien);
        }

        // POST: Technicien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Technicien == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Technicien'  is null.");
            }
            var technicien = await _context.Technicien.FindAsync(id);
            if (technicien != null)
            {
                _context.Technicien.Remove(technicien);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechnicienExists(int id)
        {
          return (_context.Technicien?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
