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
    public class ProjetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SmtpConfig _smtpConfig;

        public ProjetController(ApplicationDbContext context, SmtpConfig smtpConfig)
        {
            _context = context;
            _smtpConfig = smtpConfig;
        }

        // GET: Projet
        public IActionResult Index()
        {
            List<Projet> liste = _context.Projet
                .Include(a => a.Bailleur)
                .ToList();
            foreach (var v in liste)
            {
                VAvancementProjet vp = _context.VAvancementProjet
                    .FirstOrDefault(a => a.IdProjet == v.Id);
                if (vp != null)
                {
                    v.Avancement = vp.Avancement;
                }
                else
                {
                    v.Avancement = 0;
                }
                _context.SaveChanges();
                if (v.Avancement == 50)
                {
                    Fonction.Fonction.EnvoyerEmail(_smtpConfig,"razafimahefasteven130102@gmail.com","travisjamesmdg7713@gmail.com","Avancement a 50%","Le projet "+v.Nom+" avance a 50%");
                }

                if (v.Avancement >= 100)
                {
                    Fonction.Fonction.EnvoyerEmail(_smtpConfig,"razafimahefasteven130102@gmail.com","travisjamesmdg7713@gmail.com","Avancement a 100%","Le projet "+v.Nom+" avance a 100%");
                }
            }
            _context.SaveChanges();
            ViewData["listeprojet"] = liste;
            return View();
        }

        // GET: Projet/Details/5
        public async Task<IActionResult> Details(int? idprojet)
        {
            HttpContext.Session.SetInt32("idprojet",idprojet.GetValueOrDefault());
            if (idprojet == null || _context.Projet == null)
            {
                return NotFound();
            }

            var projet = await _context.Projet
                .Include(p => p.Bailleur)
                .FirstOrDefaultAsync(m => m.Id == idprojet);
            if (projet == null)
            {
                return NotFound();
            }

            List<TechnicienProjet> tc = _context
                .TechnicienProjet
                .Include(a => a.Technicien)
                .Where(a => a.IdProjet == idprojet).ToList();

            ViewData["listetechnicien"] = tc;
            
            List<OccurenceResultat> listeor = _context.OccurenceResultat
                .Include(a => a.Resultat)
                .Where(a => a.IdProjet == idprojet).ToList();

            foreach (var v in listeor)
            {
                var element = _context.VAvancementResultat
                    .FirstOrDefault(a => a.IdResultat == v.Id);
                if (element != null)
                {
                    v.Avancement = element.Avancement;
                }
                else
                {
                    v.Avancement = 0;
                }
            }

            _context.SaveChanges();
            ViewData["listeoccurenceresultat"] = listeor;
            
            List<ProjetComposant> liste = _context.ProjetComposant
                .Include(a => a.Composant)
                .Where(a => a.IdProjet == idprojet).ToList();
            ViewData["listecomposant"] = liste;
            
            return View(projet);
        }

        // GET: Projet/Create
        public IActionResult Create()
        {
            ViewData["listedevise"] = _context.Devise.ToList();
            ViewData["listebailleur"] = _context.Bailleur.ToList();
            ViewData["listecomposant"] = _context.Composant.ToList();
            return View();
        }

        public IActionResult CreateProjet(string nom, string details, DateOnly datedebut, DateOnly datefin, int idbailleur, List<int> idcomposant, string budget, int iddevise)
        {
            Devise devise = _context.Devise.First(a => a.Id == iddevise);
            Projet projet = new Projet()
            {
                Nom = nom,
                Details = details,
                DateDebutPrevision = datedebut,
                DateFinPrevision = datefin,
                IdBailleur = idbailleur,
                IdDevise = iddevise,
                Budget = devise.Value*Double.Parse(budget)
            };
            _context.Add(projet);
            _context.SaveChanges();
            Projet np = _context.Projet.First(a => a == projet);
            foreach (var v in idcomposant)
            {
                ProjetComposant pr = new ProjetComposant()
                {
                    IdProjet = np.Id,
                    IdComposant = v
                };
                _context.ProjetComposant.Add(pr);
            }
            
            _context.SaveChanges();
            ViewData["listeprojet"] = _context.Projet
                .Include(a => a.Bailleur)
                .ToList();
            return View("Index");
        }

        // GET: Projet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projet == null)
            {
                return NotFound();
            }

            var projet = await _context.Projet.FindAsync(id);
            if (projet == null)
            {
                return NotFound();
            }
            ViewData["IdBailleur"] = new SelectList(_context.Bailleur, "Id", "Nom", projet.IdBailleur);
            return View(projet);
        }

        // POST: Projet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,DateDebutPrevision,DateFinPrevision,FinishedOrNot,Avancement,Details,IdBailleur")] Projet projet)
        {
            if (id != projet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjetExists(projet.Id))
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
            ViewData["IdBailleur"] = new SelectList(_context.Bailleur, "Id", "Nom", projet.IdBailleur);
            return View(projet);
        }

        // GET: Projet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projet == null)
            {
                return NotFound();
            }

            var projet = await _context.Projet
                .Include(p => p.Bailleur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projet == null)
            {
                return NotFound();
            }

            return View(projet);
        }

        // POST: Projet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projet'  is null.");
            }
            var projet = await _context.Projet.FindAsync(id);
            if (projet != null)
            {
                _context.Projet.Remove(projet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjetExists(int id)
        {
          return (_context.Projet?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> AffectationTechnicien(List<int> idtechnicien)
        {
            int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
            var projet = await _context.Projet
                .Include(p => p.Bailleur)
                .FirstOrDefaultAsync(m => m.Id == idprojet);
            foreach (var v in idtechnicien)
            {
                TechnicienProjet tp = new TechnicienProjet()
                {
                    IdProjet = idprojet,
                    IdTechnicien = v
                };
                _context.Add(tp);   
            }
            _context.SaveChanges();
            List<TechnicienProjet> tc = _context
                .TechnicienProjet
                .Include(a => a.Technicien)
                .Where(a => a.IdProjet == idprojet).ToList();

            ViewData["listetechnicien"] = tc;
            
            ViewData["listeoccurenceresultat"] = _context.OccurenceResultat
                .Include(a => a.Resultat)
                .Where(a => a.IdProjet == idprojet).ToList();
            
            List<ProjetComposant> liste = _context.ProjetComposant
                .Include(a => a.Composant)
                .Where(a => a.IdProjet == idprojet).ToList();
            ViewData["listecomposant"] = liste;
            return RedirectToAction("Index", "Projet", new { projet });
        }
        
        public IActionResult VersAffectationTechnicien()
        {
            ViewData["listetechnicien"] = _context.Technicien.ToList();
            return View("AffectationTechnicien");
        }
    }
}
