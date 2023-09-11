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
        public async Task<IActionResult> Create([Bind("Id,Email,Pass")] Technicien technicien)
        {
            string token = Fonction.Fonction.GenerateToken();
            technicien.Token = token;
            _context.Add(technicien);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Pass")] Technicien technicien)
        {
            if (id != technicien.Id)
            {
                return NotFound();
            }

            try
            {
                    var technicienExist = await _context.Technicien.FindAsync(id);
                    if (technicienExist == null)
                    {
                        return NotFound();
                    }

                    technicienExist.Email = technicien.Email;
                    technicienExist.Pass = technicien.Pass;
                    
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

        public IActionResult ListeProjetTechnicien()
        {
            int idtechnicien = Int32.Parse(HttpContext.Session.GetString("idtechnicien"));
            List<Projet> liste = new List<Projet>();
            List<TechProj> listetechproj = _context.TechProj
                .Where(a => a.IdTechnicien == idtechnicien).ToList();
            foreach (var v in listetechproj)
            {
                Projet projet = _context
                    .Projet.First(a => a.Id == v.IdProjet);
                if (!liste.Contains(projet))
                {
                    liste.Add(projet);
                }
            }
            ViewData["listeprojet"] = liste;
            foreach (var v in liste)
            {
                Console.WriteLine("BLA="+v.Nom);
            }
            ViewBag.projet = "Mes projets";
            return View("~/Views/FrontTechnicien/AcceuilTech.cshtml");
        }

        public IActionResult ListeSiteTechnicien()
        {
            int idtechnicien = Int32.Parse(HttpContext.Session.GetString("idtechnicien"));
            List<Site> liste = new List<Site>();
            List<TechnicienSite> listetechsite = _context.TechnicienSite
                .Where(a => a.IdTechnicien == idtechnicien).ToList();
            foreach (var v in listetechsite)
            {
                Site site = _context.Site
                    .Include(z => z.Commune)
                    .Include(z => z.District)
                    .Include(z => z.Region)
                    .First(a => a.Id == v.IdSite);
                if (!liste.Contains(site))
                {
                    liste.Add(site);
                }
            }
            ViewData["listesite"] = liste;
            return View("~/Views/FrontTechnicien/SiteTech.cshtml");
        }

        public void versPageInsertionSite(int idoccurence, string target, int idtypeindicateur ,int idsite, int idtechnicien)
        {
            double realtarget = Double.Parse(target);
            TechnicienSite ts = new TechnicienSite()
            {
                IdSite = idsite,
                IdTechnicien = idtechnicien,
                IdIndicateur = idtypeindicateur,
                IdOccurence = idoccurence,
                Target = realtarget
            };
            _context.Add(ts);
            _context.SaveChanges();
            List<TechnicienSite> liste = _context
                .TechnicienSite
                .Include(z => z.TypeIndicateur)
                .Where(a => a.IdSite == idsite).ToList();
            ViewBag.idsite = idsite;
            ViewBag.idtechnicien = idtechnicien;
            ViewData["listetechsite"] = liste;
            // return View("~/Views/FrontTechnicien/PageInsertionRapportSurSite.cshtml");
        }

        public IActionResult versTouteLesSites()
        {
            List<Site> listesite = _context.Site
                .Include(a => a.Commune)
                .Include(a => a.District)
                .Include(a => a.Region)
                .ToList();
            ViewData["listesite"] = listesite; 
            return View("~/Views/Site/ListeSite.cshtml");
        }
    }
}