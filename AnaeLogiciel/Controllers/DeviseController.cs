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
    public class DeviseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeviseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Devise
        public async Task<IActionResult> Index()
        {
              return _context.Devise != null ? 
                          View(await _context.Devise.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Devise'  is null.");
        }

        // GET: Devise/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Devise == null)
            {
                return NotFound();
            }

            var devise = await _context.Devise
                .FirstOrDefaultAsync(m => m.Id == id);
            if (devise == null)
            {
                return NotFound();
            }

            return View(devise);
        }

        // GET: Devise/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Devise/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Value")] Devise devise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(devise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(devise);
        }

        // GET: Devise/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Devise == null)
            {
                return NotFound();
            }

            var devise = await _context.Devise.FindAsync(id);
            if (devise == null)
            {
                return NotFound();
            }
            return View(devise);
        }

        // POST: Devise/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Value")] Devise devise)
        {
            if (id != devise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviseExists(devise.Id))
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
            return View(devise);
        }

        // GET: Devise/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Devise == null)
            {
                return NotFound();
            }

            var devise = await _context.Devise
                .FirstOrDefaultAsync(m => m.Id == id);
            if (devise == null)
            {
                return NotFound();
            }

            return View(devise);
        }

        // POST: Devise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Devise == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Devise'  is null.");
            }
            var devise = await _context.Devise.FindAsync(id);
            if (devise != null)
            {
                _context.Devise.Remove(devise);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviseExists(int id)
        {
          return (_context.Devise?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
