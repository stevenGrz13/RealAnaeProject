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
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
              return _context.Admin != null ? 
                          View(await _context.Admin.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Admin'  is null.");
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Pass")] Admin admin)
        {
            string token = Fonction.Fonction.GenerateToken();
            admin.Token = token;
            _context.Add(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Pass")] Admin admin)
        {
            if (id != admin.Id)
            {
                return NotFound();
            }

            try
            {
                var adminExist = await _context.Admin.FindAsync(id);
                if (adminExist == null)
                {
                    return NotFound();
                }

                adminExist.Email = admin.Email;
                adminExist.Pass = admin.Pass;

                _context.Update(adminExist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(admin.Id))
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

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin
                .FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Admin == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Admin'  is null.");
            }
            var admin = await _context.Admin.FindAsync(id);
            if (admin != null)
            {
                _context.Admin.Remove(admin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
          return (_context.Admin?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public IActionResult Login(int idtypeuser, string email, string pass)
        {
            Console.WriteLine("idtypeuser="+idtypeuser);
            if (idtypeuser == 1)
            {
                List<Admin> liste = _context.Admin.ToList();
                foreach (var v in liste)
                {
                    if (v.Email == email && v.Pass == pass)
                    {
                        HttpContext.Session.SetString("idadmin",v.Id+"");
                        Console.WriteLine("makato amle page frontofficeacceuil");
                        return View("~/Views/FrontOffice/Acceuil.cshtml");  
                    }
                }   
            }
            if (idtypeuser == 2)
            {
                List<Employe> liste = _context.Employe.ToList();
                foreach (var v in liste)
                {
                    if (v.Email == email && v.Pass == pass)
                    {
                        HttpContext.Session.SetString("idemploye",v.Id+"");
                        Console.WriteLine("makato amle page frontofficeacceuil");
                        return View("~/Views/FrontOffice/Acceuil.cshtml");  
                    }
                }   
            }
            ViewBag.message = "verifiez vos informations de connexion";
            return View("~/Views/Home/Index.cshtml"); 
        }
        
        [HttpPost]
        public IActionResult Deconnexion()
        {
            HttpContext.Session.Remove("idadmin");
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
