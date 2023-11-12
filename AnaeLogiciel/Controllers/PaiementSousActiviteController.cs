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

    public IActionResult VersListePaiementSousActivite(int idoccurencesousactivite, int? iddevise)
    {
        if (iddevise == null)
        {
            iddevise = 1;
        }
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        Projet projet = _context.Projet
            .Include(a => a.Devise)
            .First(a => a.Id == idprojet);
        ViewBag.nomdevise = projet.Devise.Nom;
        List<PaiementOccurenceSousActivite> liste = _context
            .PaiementOccurenceSousActivite
            .Include(a => a.Technicien)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
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
        
        ViewData["listepaiement"] = liste;
        
        string messageerreur = "";
        if (reste < 0)
        {
            messageerreur = "attention budget deja depassee";
        }

        ViewBag.messageerreur = messageerreur;
        ViewBag.idoccurenceactivite = os.IdOccurenceActivite;
        List<Devise> listedevise = _context.Devise
            .Where(a => a.Id == projet.IdDevise)
            .ToList();
        Devise ariary = _context.Devise.First(a => a.Id == 1);
        listedevise.Add(ariary);
        ViewData["listedevise"] = listedevise;

        return View("~/Views/PaiementSousActivite/Liste.cshtml");
    }

    public IActionResult VersInsertionPaiementSousActivite(int idoccurencesousactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.mesageerreur = TempData["messageerreur"];
        }
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
            PaiementOccurenceSousActivite po = new PaiementOccurenceSousActivite()
            {
                IdOccurenceSousActivite = idoccurencesousactivite,
                IdTechnicien = idtechnicien,
                Montant = Double.Parse(montant) * projet.ValeurDevise,
                Motif = motif,
                DateAction = dateaction
            };
            _context.Add(po);
            _context.SaveChanges();
            return RedirectToAction("VersListePaiementSousActivite",
                new { idoccurencesousactivite = idoccurencesousactivite });   
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionPaiementSousActivite",
                new { idoccurencesousactivite = idoccurencesousactivite });
        }
    }
}