using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;

namespace AnaeLogiciel.Controllers;

public class PaiementSousActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaiementSousActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersListePaiementSousActivite(int idoccurencesousactivite)
    {
        List<PaiementOccurenceSousActivite> liste = _context
            .PaiementOccurenceSousActivite
            .Include(a => a.Technicien)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        ViewData["listepaiement"] = liste;
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        OccurenceSousActivite os = _context.OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite);
        ViewBag.budget = os.Budget;
        double total = 0;
        double reste = 0;
        foreach (var v in liste)
        {
            total += v.Montant;
        }

        reste = os.Budget - total;
        ViewBag.total = total;
        ViewBag.budget = os.Budget;
        ViewBag.reste = reste;
        string messageerreur = "";
        if (reste < 0)
        {
            messageerreur = "attention budget deja depassee";
        }

        ViewBag.messageerreur = messageerreur;
        return View("~/Views/PaiementSousActivite/Liste.cshtml");
    }

    public IActionResult VersInsertionPaiementSousActivite(int idoccurencesousactivite)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        List<TechnicienProjet> listetech = _context.TechnicienProjet
            .Include(a => a.Technicien)
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        ViewData["listetechnicien"] = listetech;
        return View("~/Views/PaiementSousActivite/Insertion.cshtml");
    }

    public IActionResult Create(int idoccurencesousactivite, int idtechnicien, string montant, string motif, DateOnly dateaction)
    {
        PaiementOccurenceSousActivite po = new PaiementOccurenceSousActivite()
        {
            IdOccurenceSousActivite = idoccurencesousactivite,
            IdTechnicien = idtechnicien,
            Montant = Double.Parse(montant),
            Motif = motif,
            DateAction = dateaction
        };
        _context.Add(po);
        _context.SaveChanges();
        List<PaiementOccurenceSousActivite> liste = _context
            .PaiementOccurenceSousActivite
            .Include(a => a.Technicien)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        ViewData["listepaiement"] = liste;
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        OccurenceSousActivite os = _context.OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite);
        ViewBag.budget = os.Budget;
        double total = 0;
        double reste = 0;
        foreach (var v in liste)
        {
            total += v.Montant;
        }

        reste = os.Budget - total;
        ViewBag.total = total;
        ViewBag.budget = os.Budget;
        ViewBag.reste = reste;
        string messageerreur = "";
        if (reste < 0)
        {
            messageerreur = "attention budget deja depassee";
        }

        ViewBag.messageerreur = messageerreur;
        return View("~/Views/PaiementSousActivite/Liste.cshtml");
    }
}