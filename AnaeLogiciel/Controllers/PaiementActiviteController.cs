using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class PaiementActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaiementActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersListePaiementActivite(int idoccurenceactivite, int? iddevise)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        Projet projet = _context.Projet
            .First(a => a.Id == idprojet);
        if (iddevise == null)
        {
            iddevise = 1;
        }
        double totalprolongement = 0;
        List<ProlongementBudgetOccurenceActivite> listeprolongement = _context
            .ProlongementBudgetOccurenceActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        foreach (var v in listeprolongement)
        {
            totalprolongement += v.Budget;
        }
        List<PaiementOccurenceActivite> liste = _context
            .PaiementOccurenceActivite
            .Include(a => a.Technicien)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        OccurenceActivite os = _context.OccurenceActivite
            .First(a => a.Id == idoccurenceactivite);
        ViewBag.budget = os.Budget;
        double total = 0;
        double reste = 0;
        foreach (var v in liste)
        {
            total += v.Montant;
        }

        reste = (os.Budget+totalprolongement) - total;

        if (iddevise == 1)
        {
            ViewBag.total = total;
            ViewBag.budget = os.Budget;
            ViewBag.reste = reste;
        }
        else
        {
            foreach (var v in liste)
            {
                v.Montant = v.Montant / projet.ValeurDevise;
            }
            ViewBag.total = total / projet.ValeurDevise;
            ViewBag.budget = os.Budget / projet.ValeurDevise;
            ViewBag.reste = reste / projet.ValeurDevise;
        }
        
        string messageerreur = "";
        if (reste < 0)
        {
            messageerreur = "attention budget deja depassee";
        }

        ViewBag.messageerreur = messageerreur;
        ViewBag.idoccurenceresultat = os.IdOccurenceResultat;
        ViewData["listepaiement"] = liste;
        List<Devise> listedevise = _context.Devise
            .Where(a => a.Id == projet.IdDevise)
            .ToList();
        Devise ariary = _context.Devise.First(a => a.Id == 1);
        listedevise.Add(ariary);
        ViewData["listedevise"] = listedevise;
        return View("~/Views/PaiementActivite/Liste.cshtml");
    }

    public IActionResult VersInsertionPaiementActivite(int idoccurenceactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        List<TechnicienProjet> listetech = _context.TechnicienProjet
            .Include(a => a.Technicien)
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        ViewData["listetechnicien"] = listetech;
        return View("~/Views/PaiementActivite/Insertion.cshtml");
    }

    public IActionResult Create(int idoccurenceactivite, int idtechnicien, string montant, string motif, DateOnly dateaction)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        Projet projet = _context.Projet
            .First(a => a.Id == idprojet);
        string messageerreur = "";
        try
        {
            Double.Parse(montant);
        }
        catch (Exception e)
        {
            messageerreur += "- montant invalide -";
        }

        if (messageerreur == "")
        {
            PaiementOccurenceActivite po = new PaiementOccurenceActivite()
            {
                IdOccurenceActivite = idoccurenceactivite,
                IdTechnicien = idtechnicien,
                Montant = Double.Parse(montant) * projet.ValeurDevise,
                Motif = motif,
                DateAction = dateaction
            };
            _context.Add(po);
            _context.SaveChanges();
            return RedirectToAction("VersListePaiementActivite", new {idoccurenceactivite = idoccurenceactivite});   
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionPaiementActivite", new { idoccurenceactivite = idoccurenceactivite });
        }
    }
}