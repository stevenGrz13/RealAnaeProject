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

    public IActionResult VersListePaiementActivite(int idoccurenceactivite)
    {
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
        ViewData["listepaiement"] = liste;
        ViewData["listeprolongement"] = listeprolongement;
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
        ViewBag.total = total;
        ViewBag.budget = os.Budget;
        ViewBag.reste = reste;
        string messageerreur = "";
        if (reste < 0)
        {
            messageerreur = "attention budget deja depassee";
        }

        ViewBag.messageerreur = messageerreur;
        return View("~/Views/PaiementActivite/Liste.cshtml");
    }

    public IActionResult VersInsertionPaiementActivite(int idoccurenceactivite)
    {
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
        PaiementOccurenceActivite po = new PaiementOccurenceActivite()
        {
            IdOccurenceActivite = idoccurenceactivite,
            IdTechnicien = idtechnicien,
            Montant = Double.Parse(montant),
            Motif = motif,
            DateAction = dateaction
        };
        _context.Add(po);
        _context.SaveChanges();
        List<PaiementOccurenceActivite> liste = _context
            .PaiementOccurenceActivite
            .Include(a => a.Technicien)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewData["listepaiement"] = liste;
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

        reste = os.Budget - total;
        string messageerreur = "";
        if (reste < 0)
        {
            messageerreur = "attention budget deja depassee";
        }

        ViewBag.messageerreur = messageerreur;
        ViewBag.total = total;
        ViewBag.budget = os.Budget;
        ViewBag.reste = reste;
        return View("~/Views/PaiementActivite/Liste.cshtml");
    }
}