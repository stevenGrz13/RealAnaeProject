using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class BackOfficeSousActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public BackOfficeSousActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IActionResult VersRapportSousActivite(int idtechnicien, int idsitesousactivite, int idindicateur)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        ViewData["technicien"] = _context.Technicien
            .First(a => a.Id == idtechnicien);
        ViewData["indicateur"] = _context.TypeIndicateur
            .First(a => a.Id == idindicateur);
        ViewData["sitesousactivite"] = _context
            .SiteSousActivite.First(a => a.Id == idsitesousactivite);
        return View("~/Views/RapportIndicateurSousActivite/Insertion.cshtml");
    }

    public IActionResult Create(int idtechnicien, int idsitesousactivite, int idindicateur, string quantite, DateOnly dateaction)
    {
        string messageerreur = "";
        try
        {
            Double.Parse(quantite);
        }
        catch (Exception e)
        {
            messageerreur += "- Quantite invalide -";
        }

        if (messageerreur == "")
        {
            RapportIndicateurSousActivite ri = new RapportIndicateurSousActivite()
            {
                IdTechnicien = idtechnicien,
                IdSiteSousActivite = idsitesousactivite,
                IdIndicateur = idindicateur,
                Quantite = Double.Parse(quantite),
                DateAction = dateaction
            };
            _context.Add(ri);
            _context.SaveChanges();
            return RedirectToAction("VersDetailsSiteSousActivite", "SiteSousActivite",
                new { idsitesousactivite = idsitesousactivite });   
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersRapportSousActivite",
                new
                {
                    idtechnicien = idtechnicien, idsitesousactivite = idsitesousactivite, idindicateur = idindicateur
                });
        }
    }
    
    public IActionResult VersListeRapportSousActivite(int idsitesousactivite)
    {
        int idoccurencesousactivite = _context.SiteSousActivite
            .First(a => a.Id == idsitesousactivite).IdOccurenceSousActivite;
        int idoccurenceactivite = _context.OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite)
            .IdOccurenceActivite;
        List<RapportIndicateurSousActivite> liste = _context
            .RapportIndicateurSousActivite
            .Include(a => a.TypeIndicateur)
            .Include(a => a.Technicien)
            .Where(a => a.IdSiteSousActivite == idsitesousactivite)
            .ToList();
        ViewData["listerapportsite"] = liste;
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/RapportIndicateurSousActivite/Liste.cshtml");
    }
}