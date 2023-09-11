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
    public class EmployeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employe
        public async Task<IActionResult> Index()
        {
              return _context.Employe != null ? 
                          View(await _context.Employe.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Employe'  is null.");
        }

        // GET: Employe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employe == null)
            {
                return NotFound();
            }

            var employe = await _context.Employe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employe == null)
            {
                return NotFound();
            }

            return View(employe);
        }

        // GET: Employe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Pass")] Employe employe)
        {
            string token = Fonction.Fonction.GenerateToken();
            employe.Token = token;
            _context.Add(employe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Employe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employe == null)
            {
                return NotFound();
            }

            var employe = await _context.Employe.FindAsync(id);
            if (employe == null)
            {
                return NotFound();
            }
            return View(employe);
        }

        // POST: Employe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Pass")] Employe employe)
        {
            if (id != employe.Id)
            {
                return NotFound();
            }

            try
            {
                var employeExist = await _context.Employe.FindAsync(id);
                if (employeExist == null)
                {
                    return NotFound();
                }

                employeExist.Email = employe.Email;
                employeExist.Pass = employe.Pass;

                _context.Update(employeExist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeExists(employe.Id))
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


        // GET: Employe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employe == null)
            {
                return NotFound();
            }

            var employe = await _context.Employe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employe == null)
            {
                return NotFound();
            }

            return View(employe);
        }

        // POST: Employe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employe == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employe'  is null.");
            }
            var employe = await _context.Employe.FindAsync(id);
            if (employe != null)
            {
                _context.Employe.Remove(employe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeExists(int id)
        {
          return (_context.Employe?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
