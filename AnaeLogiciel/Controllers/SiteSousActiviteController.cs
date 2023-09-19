using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class SiteSousActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public SiteSousActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IActionResult VersInsertionSiteSousActivite(int idoccurencesousactivite)
    {
        ViewData["listecommune"] = _context.Commune.ToList();
        ViewData["listedistrict"] = _context.District.ToList();
        ViewData["listeregion"] = _context.Region.ToList();
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        return View("~/Views/SiteSousActivite/Insertion.cshtml");
    }

    public IActionResult Create(string libelle, int commune, int region, int district, int idoccurencesousactivite)
    {
        SiteSousActivite st = new SiteSousActivite()
        {
            IdOccurenceSousActivite = idoccurencesousactivite,
            Libelle = libelle,
            IdCommune = commune,
            IdDistrict = district,
            IdRegion = region
        };
        _context.Add(st);
        _context.SaveChanges();
        ViewData["listesiteoccurencesousactivite"] = _context.SiteSousActivite
            .Include(a => a.Commune)
            .Include(a => a.District)
            .Include(a => a.Region)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite).ToList();
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        ViewData["listesiteoccurencesousactivite"] = _context.OccurenceSousActiviteIndicateur
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        return View("~/Views/OccurenceSousActivite/Details.cshtml");
    }
}