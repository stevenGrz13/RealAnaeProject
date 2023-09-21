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
        ViewData["technicien"] = _context.Technicien
            .First(a => a.Id == idtechnicien);
        ViewData["indicateur"] = _context.TypeIndicateur
            .First(a => a.Id == idindicateur);
        ViewData["sitesousactivite"] = _context
            .SiteSousActivite.First(a => a.Id == idsitesousactivite);
        return View("~/Views/RapportIndicateurSousActivite/Insertion.cshtml");
    }

    public void Create(int idtechnicien, int idsitesousactivite, int idindicateur, string quantite, DateOnly dateaction)
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
    }
    
    public IActionResult VersListeRapportSousActivite(int idsitesousactivite)
    {
        List<RapportIndicateurSousActivite> liste = _context
            .RapportIndicateurSousActivite
            .Include(a => a.TypeIndicateur)
            .Include(a => a.Technicien)
            .Where(a => a.IdSiteSousActivite == idsitesousactivite)
            .ToList();
        ViewData["listerapportsite"] = liste;
        return View("~/Views/RapportIndicateurSousActivite/Liste.cshtml");
    }
}