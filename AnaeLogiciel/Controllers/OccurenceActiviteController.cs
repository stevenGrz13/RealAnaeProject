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
    public class OccurenceActiviteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OccurenceActiviteController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: ActiviteProjetController
        public async Task<IActionResult> Index()
        {
            List<OccurenceActivite> listeactiviteprojet = _context.ActiviteProjet
                .Include(a => a.Activite)
                .Include(a => a.Projet)
                .ToList();
            return View(listeactiviteprojet);
        }

        // GET: ActiviteProjetControllet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActiviteProjet == null)
            {
                return NotFound();
            }

            var activiteProjet = await _context.ActiviteProjet
                .Include(a => a.Activite)
                .Include(a => a.Projet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activiteProjet == null)
            {
                return NotFound();
            }

            return View(activiteProjet);
        }

        // GET: ActiviteProjetControllet/Create
        public IActionResult Create()
        {
            ViewData["IdActivite"] = new SelectList(_context.Activite, "Id", "Nom");
            ViewData["IdProjet"] = new SelectList(_context.Projet, "Id", "Nom");
            return View();
        }

        public IActionResult RealIndex(int? idprojet)
        {
            if (idprojet == null)
            {
                idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
            }
            else
            {
                HttpContext.Session.SetString("idprojet", idprojet+"");
                idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
            }
            Projet projet = _context.Projet.First(p => p.Id == idprojet);
            List<OccurenceActivite> listeactiviteprojet = _context.ActiviteProjet
                .Include(p => p.Activite)
                .Where(activiteProjet => activiteProjet.IdProjet == idprojet).ToList();
            
            foreach (var v in listeactiviteprojet)
            {
                List<AssociationIndicateurActivite> x =
                    _context.IndicateurActiviteProjet.Where(a =>
                        a.IdOccurence == v.Id).ToList();
                
                double[] quantitedemandeeparindicateur = new double[x.Count()];

                for (int i = 0; i < quantitedemandeeparindicateur.Length; i++)
                {
                    quantitedemandeeparindicateur[i] = x[i].QuantiteDemande;
                }
                
                double[] quantiteeffectueeparindicateur = new double[x.Count()];

                for (int i = 0; i < x.Count; i++)
                {
                    try
                    {
                        double quantite = 0;
                        List<RapportTechnicienSite> associationIndicateurActiviteAvecDate =
                            _context.RapportTechnicienSite
                                .Where(a => a.IdOccurence == v.Id && a.IdIndicateur == x[i].IdTypeIndicateur)
                                .ToList();
                        foreach (var variable in associationIndicateurActiviteAvecDate)
                        {
                            quantite += variable.TargetEffectue;
                        }

                        Console.WriteLine("somme effectue="+quantite);
                        quantiteeffectueeparindicateur[i] = quantite;
                    }
                    catch (Exception e)
                    {
                        quantiteeffectueeparindicateur[i] = 0;
                    }
                }

                double[] avancement = new double[x.Count()];

                for (int j = 0; j < x.Count; j++)
                {
                    avancement[j] = (quantiteeffectueeparindicateur[j] * 100) / quantitedemandeeparindicateur[j];
                }

                double avancementglobal = 0;

                for (int i = 0; i < x.Count(); i++)
                {
                    avancementglobal += avancement[i];
                }

                double realavancement = avancementglobal / x.Count;
                
                if (Double.IsNaN(realavancement))
                {
                    v.Avancement = 0;   
                }
                else
                {
                    v.Avancement = realavancement;
                }
                _context.SaveChanges();
                
                if (v.Avancement >= 100)
                {
                    v.FinishedOrNot = true;
                }
                else
                {
                    v.FinishedOrNot = false;
                }

                _context.SaveChanges();
            }
            
            ViewData["projet"] = projet;
            ViewData["listeactivite"] = listeactiviteprojet;
            return View();
        }

        // POST: ActiviteProjetController/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdProjet,IdActivite,DateDebutPrevision,DateFinPrevision,Budget,FinishedOrNot")] OccurenceActivite occurenceActivite)
        {
            Projet projet = _context.Projet
                .First(p => p.Id == occurenceActivite.IdProjet);

            if (Fonction.Fonction.ValidateDates(projet.DateDebutPrevision, projet.DateFinPrevision,
                    occurenceActivite.DateDebutPrevision, occurenceActivite.DateFinPrevision))
            {
                if (ModelState.IsValid)
                {
                    occurenceActivite.Avancement = 0;
                    _context.Add(occurenceActivite);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdActivite"] = new SelectList(_context.Activite, "Id", "Nom", occurenceActivite.IdActivite);
                ViewData["IdProjet"] = new SelectList(_context.Projet, "Id", "Nom", occurenceActivite.IdProjet);
                return View(occurenceActivite);
            }
            else
            {
                ViewBag.messageerreur = "date invalide au projet";
                ViewData["IdActivite"] = new SelectList(_context.Activite, "Id", "Nom", occurenceActivite.IdActivite);
                ViewData["IdProjet"] = new SelectList(_context.Projet, "Id", "Nom", occurenceActivite.IdProjet);
                return View(occurenceActivite);
            }
        }

        // GET: ActiviteProjetControllet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActiviteProjet == null)
            {
                return NotFound();
            }

            var activiteProjet = await _context.ActiviteProjet.FindAsync(id);
            if (activiteProjet == null)
            {
                return NotFound();
            }
            ViewData["IdActivite"] = new SelectList(_context.Activite, "Id", "Nom", activiteProjet.IdActivite);
            ViewData["IdProjet"] = new SelectList(_context.Projet, "Id", "Nom", activiteProjet.IdProjet);
            return View(activiteProjet);
        }

        // POST: ActiviteProjetControllet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdProjet,IdActivite,DateDebutPrevision,DateFinPrevision,Budget,FinishedOrNot")] OccurenceActivite occurenceActivite)
        {
            if (id != occurenceActivite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(occurenceActivite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActiviteProjetExists(occurenceActivite.Id))
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
            ViewData["IdActivite"] = new SelectList(_context.Activite, "Id", "Nom", occurenceActivite.IdActivite);
            ViewData["IdProjet"] = new SelectList(_context.Projet, "Id", "Nom", occurenceActivite.IdProjet);
            return View(occurenceActivite);
        }

        // GET: ActiviteProjetControllet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActiviteProjet == null)
            {
                return NotFound();
            }

            var activiteProjet = await _context.ActiviteProjet
                .Include(a => a.Activite)
                .Include(a => a.Projet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activiteProjet == null)
            {
                return NotFound();
            }

            return View(activiteProjet);
        }

        // POST: ActiviteProjetControllet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActiviteProjet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ActiviteProjet'  is null.");
            }
            var activiteProjet = await _context.ActiviteProjet.FindAsync(id);
            if (activiteProjet != null)
            {
                _context.ActiviteProjet.Remove(activiteProjet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActiviteProjetExists(int id)
        {
          return (_context.ActiviteProjet?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult AjouterActivite(int? idprojet)
        {
            if (idprojet == null)
            {
                idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
            }
            else
            {
                HttpContext.Session.SetString("idprojet", idprojet+"");
                idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
            }
            if (TempData["messageerreur"] != null)
            {
                ViewBag.messageerreur = TempData["messageerreur"];   
            }
            Projet projet = _context
                .Projet.First(p => p.Id == idprojet);
            List<Activite> listeactivite = _context.Activite.ToList();
            List<SourceDeVerification> listesourcedeverification = _context.SourceDeVerification
                .ToList();
            ViewData["projet"] = projet;
            ViewData["listeactivite"] = listeactivite;
            ViewData["listesourcedeverification"] = listesourcedeverification;
            return View("InsertionActiviteProjet");
        }

        public IActionResult InsertionGlobalActiviteProjet(int idprojet,DateOnly datedebutprevision,DateOnly datefinprevision,string budget,int idactivite,List<int> listesource)
        {
            HttpContext.Session.SetString("idprojet", idprojet.ToString());
            idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
            double montant = 0;
            string messageerreur = "";
         
            Projet prj = _context
                .Projet
                .First(p => p.Id == idprojet);

            if (!Fonction.Fonction.ValidateDates(prj.DateDebutPrevision, prj.DateFinPrevision, datedebutprevision,
                    datefinprevision))
            {
                messageerreur += " - date invalide - ";
            }

            try
            {
                montant = Double.Parse(budget);
                if (montant <= 0)
                {
                    messageerreur += " - budget inferieur ou egal a 0 n'est pas valable - ";
                }
            }
            catch (Exception e)
            {
                messageerreur += " - budget invalide - ";
            }

            if (messageerreur == "")
            {
                OccurenceActivite ap = new OccurenceActivite()
                {
                    IdProjet = idprojet,
                    IdActivite = idactivite,
                    DateDebutPrevision = datedebutprevision,
                    DateFinPrevision = datefinprevision,
                    Budget = montant,
                    Avancement = 0
                };
                _context.Add(ap);
                _context.SaveChanges();
                TempData["idprojet"] = idprojet;

                OccurenceActivite insertedOccurence = _context.ActiviteProjet
                    .First(a => a == ap);

                foreach (var v in listesource)
                {
                    SourceDeVerification src1 = _context.SourceDeVerification
                        .First(a => a.Id == v);
                    Fonction.Fonction.CreerDossier(_webHostEnvironment,insertedOccurence.Id, src1.Nom);
                    OccurenceSourceDeVerification src = new OccurenceSourceDeVerification()
                    {
                        IdOccurence = insertedOccurence.Id,
                        IdSourcedeverification = v
                    };
                    _context.Add(src);
                    _context.SaveChanges();
                }

                return RedirectToAction("RealIndex");
            }
            else
            {
                TempData["idprojet"] = idprojet;
                TempData["messageerreur"] = messageerreur;
                Console.WriteLine(messageerreur);
                return RedirectToAction("AjouterActivite");
            }
        }
    }
}
