using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class BackOfficeActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public BackOfficeActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersRapportActivite(int idtechnicien, int idsiteactivite, int idindicateur)
    {
        ViewData["technicien"] = _context.Technicien
            .First(a => a.Id == idtechnicien);
        ViewData["indicateur"] = _context.TypeIndicateur
            .First(a => a.Id == idindicateur);
        ViewData["siteactivite"] = _context
            .SiteActivite.First(a => a.Id == idsiteactivite);
        return View("~/Views/RapportIndicateurActivite/Insertion.cshtml");
    }

    public IActionResult Create(int idtechnicien, int idsiteactivite, int idindicateur, string quantite, DateOnly dateaction)
    {
        RapportIndicateurActivite ri = new RapportIndicateurActivite()
        {
            IdTechnicien = idtechnicien,
            IdSiteActivite = idsiteactivite,
            IdIndicateur = idindicateur,
            Quantite = Double.Parse(quantite),
            DateAction = dateaction
        };
        _context.Add(ri);
        _context.SaveChanges();
        return RedirectToAction("VersDetailsSiteActivite", "SiteActivite", new { idsiteactivite = idsiteactivite });
    }

    public IActionResult VersListeRapportActivite(int idsiteactivite)
    {
        int idoccurenceactivite = _context.SiteActivite
            .First(a => a.Id == idsiteactivite).IdOccurenceActivite;
        List<RapportIndicateurActivite> liste = _context
            .RapportIndicateurActivite
            .Include(a => a.TypeIndicateur)
            .Include(a => a.Technicien)
            .Where(a => a.IdSiteActivite == idsiteactivite)
            .ToList();
        ViewData["listerapportsite"] = liste;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/RapportIndicateurActivite/Liste.cshtml");
    }
}