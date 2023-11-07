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
        public IActionResult Index(int? page, string? search)
        {
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 3;
            IQueryable<Projet> query;
            if (search == null)
            {
                query = _context.Projet.Where(a => a.IsSupp == false);
            }
            else
            {
                query = _context.Projet
                    .Where(a => a.Sigle.Contains(search) && a.IsSupp == false);
            }
                
            int totalItems = query.Count();

            var pagedList = query
                .OrderBy(a => a.Id)
                .Skip((page.GetValueOrDefault() - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new PagedList<Projet>
            {
                Items = pagedList,
                TotalItems = totalItems,
                PageNumber = page.GetValueOrDefault(),
                PageSize = pageSize
            };
            
            ViewData["listeprojet"] = model;
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

            List<OccurenceResultat> listeor = new List<OccurenceResultat>();
            
            listeor = _context.OccurenceResultat
                .Where(a => a.IdProjet == idprojet && a.IsSupp == false).ToList();

            List<TechnicienProjet> tc = _context
                .TechnicienProjet
                .Include(a => a.Technicien)
                .Where(a => a.IdProjet == idprojet).ToList();
            
            ViewData["listetechnicien"] = tc;
            
            ViewData["listeoccurenceresultat"] = listeor;
            
            List<ProlongementProjet> listeprolongement = _context.ProlongementProjet
                .Where(a => a.IdProjet == idprojet).ToList();

            double sommeprolongement = 0;
            
            List<ProlongementBudgetProjet> listeprolongementbudget = _context
                .ProlongementBudgetProjet
                .Where(a => a.IdProjet == idprojet)
                .ToList();

            if (listeprolongement.Count > 0)
            {
                projet.DateFinPrevision = listeprolongement.LastOrDefault().DateFin;
            }

            if (listeprolongementbudget.Count > 0)
            {
                foreach (var v in listeprolongementbudget)
                {
                    sommeprolongement += v.Budget;
                }

                projet.Budget += sommeprolongement;
            }
            
            if (Fonction.Fonction.getDateNow() > projet.DateFinPrevision && projet.Avancement != 100)
            {
                projet.Couleur = "text-danger";
                projet.Message = "En retard";
            }
            else
            {
                projet.Couleur = "text-success";
                projet.Message = "A temps";
            }
            
            _context.SaveChanges();
            ViewData["listeoccurenceresultat"] = listeor;
            
            List<ProjetComposant> liste = _context.ProjetComposant
                .Where(a => a.IdProjet == idprojet).ToList();
            ViewData["listecomposant"] = liste;

            ViewData["listepartenaire"] = _context.ProjetPartenaireTechnique
                .Where(a => a.IdProjet == idprojet)
                .ToList();

            return View(projet);
        }

        public IActionResult AjoutComposant(string Composant)
        {
            int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
            ProjetComposant pr = new ProjetComposant()
            {
                Composant = Composant,
                IdProjet = idprojet
            };
            _context.Add(pr);
            _context.SaveChanges();
            Console.WriteLine("AVY NISAVE IZY ZAO");
            return RedirectToAction("Details", new {idprojet = idprojet});
        }
        
        // GET: Projet/Create
        public IActionResult Create(string messageerreur)
        {
            ViewBag.messageerreur = messageerreur;
            ViewData["listedevise"] = _context.Devise.ToList();
            ViewData["listebailleur"] = _context.Bailleur.ToList();
            return View();
        }

        public IActionResult CreateProjet(string reference, string sigle,string nom, string details, DateOnly datedebut, DateOnly datefin, int idbailleur, string budget, int iddevise, string valeur)
        {
            string messageerreur = "";
            try
            {
                Double.Parse(budget);
            }
            catch (Exception e)
            {
                messageerreur += "- montant invalide -";
            }

            try
            {
                Double.Parse(valeur);
            }
            catch (Exception e)
            {
                messageerreur += "- valeur devise invalide -";
            }
            
            if (!Fonction.Fonction.SecureDate(datedebut, datefin))
            {
                messageerreur += "- dates invalide -";
            }

            if (messageerreur == "")
            {
                Projet projet = new Projet()
                {
                    Nom = nom,
                    Details = details,
                    DateDebutPrevision = datedebut,
                    DateFinPrevision = datefin,
                    IdBailleur = idbailleur,
                    IdDevise = iddevise,
                    ValeurDevise = Double.Parse(valeur),
                    Budget = Double.Parse(valeur)*Double.Parse(budget),
                    Sigle = sigle,
                    Reference = reference
                };
                _context.Add(projet);
                _context.SaveChanges();
                Projet np = _context.Projet.First(a => a == projet);

                RealDataProjet rl = new RealDataProjet()
                {
                    IdProjet = np.Id,
                    DateFin = np.DateFinPrevision,
                    Budget = np.Budget
                };
                
                _context.Add(rl);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Create", new {messageerreur = messageerreur});
            }
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Details,IdBailleur")] Projet projet)
        {
            if (id != projet.Id)
            {
                return NotFound();
            }
            Projet pr = _context.Projet
                .First(a => a.Id == id);
            pr.Nom = projet.Nom;
            pr.Details = projet.Details;
            pr.IdBailleur = projet.IdBailleur;
            _context.SaveChanges();
            return RedirectToAction("Details", new {idprojet = id});
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
                projet.IsSupp = true;
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
                .Where(a => a.IdProjet == idprojet).ToList();
            
            List<ProjetComposant> liste = _context.ProjetComposant
                .Where(a => a.IdProjet == idprojet).ToList();
            ViewData["listecomposant"] = liste;
            return RedirectToAction("Details",new {idprojet = idprojet});
        }
        
        public IActionResult VersAffectationTechnicien()
        {
            ViewData["listetechnicien"] = _context.Technicien.ToList();
            return View("AffectationTechnicien");
        }

        public IActionResult VersInsertionPartenaireTechnique(int idprojet)
        {
            ViewBag.idprojet = idprojet;
            ViewData["listepartenaire"] = _context.ProjetPartenaireTechnique
                .Where(a => a.IdProjet == idprojet)
                .ToList();
            return View("PageInsertionPartenaireTechnique");
        }

        public IActionResult InsertionPartenaire(string partenairetechnique, int idprojet)
        {
            ProjetPartenaireTechnique pt = new ProjetPartenaireTechnique()
            {
                IdProjet = idprojet,
                PartenaireTechnique = partenairetechnique
            };
            _context.Add(pt);
            _context.SaveChanges();
            return RedirectToAction("Details", new { idprojet = idprojet });
        }

        public IActionResult RetourVersDetails(int idprojet)
        {
            return RedirectToAction("Details", new { idprojet = idprojet });
        } 
    }
}
