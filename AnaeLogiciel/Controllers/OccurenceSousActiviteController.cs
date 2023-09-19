using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class OccurenceSousActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public OccurenceSousActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult ListeOccurenceSousActivite(int idoccurenceactivite)
    {
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        ViewData["listeoccurencesousactivite"] = _context.OccurenceSousActivite
            .Include(a => a.SousActivite)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        return View("~/Views/OccurenceSousActivite/Liste.cshtml");
    }

    public IActionResult VersInsertionOccurenceSousActivite(int idoccurenceactivite)
    {
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        ViewData["listesousactivite"] = _context.SousActivite.ToList();
        return View("~/Views/OccurenceSousActivite/Insertion.cshtml");
    }

    public void Create(int idoccurenceactivite, int idsousactivite, string budget, DateOnly datedebut, DateOnly datefin)
    {
        OccurenceSousActivite osc = new OccurenceSousActivite()
        {
            IdOccurenceActivite = idoccurenceactivite,
            IdSousActivite = idsousactivite,
            Budget = Double.Parse(budget),
            DateDebut = datedebut,
            DateFin = datefin
        };
        _context.Add(osc);
        _context.SaveChanges();
    }

    public IActionResult VersDetailsOccurenceSousActivite(int idoccurencesousactivite)
    {
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        ViewData["listesiteoccurencesousactivite"] = _context.SiteSousActivite
            .Include(a => a.Commune)
            .Include(a => a.Region)
            .Include(a => a.District)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite).ToList();
        ViewData["listeoccurencesousactiviteindicateur"] = _context
            .OccurenceSousActiviteIndicateur
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        return View("~/Views/OccurenceSousActivite/Details.cshtml");
    }

    public IActionResult VersDetailsSousActiviteSite(int idsitesousactivite)
    {
        return View("~/Views/OccurenceSousActivite/Details.cshtml");
    }
}