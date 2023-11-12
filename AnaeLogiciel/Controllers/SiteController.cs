using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class SiteController : Controller
{
    private readonly ApplicationDbContext _context;

    public SiteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersListeSiteProjet()
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        List<Site> liste = _context.Site
            .Include(a => a.Commune)
            .Include(a => a.Region)
            .Include(a => a.District)
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        ViewData["listesite"] = liste;
        return View("~/Views/SiteProjet/Liste.cshtml");
    }

    public IActionResult VersInsertion()
    {
        List<Commune> listecommune = _context.Commune.ToList();
        List<Region> listeregion = _context.Region.ToList();
        List<District> listedistrict = _context.District.ToList();
        ViewData["listecommune"] = listecommune;
        ViewData["listeregion"] = listeregion;
        ViewData["listedistrict"] = listedistrict;
        return View("~/Views/SiteProjet/Insertion.cshtml");
    }

    public IActionResult Create(int commune, int district, int region, string details)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        Site site = new Site()
        {
            IdProjet = idprojet,
            IdCommune = commune,
            IdDistrict = district,
            IdRegion = region,
            Details = details
        };
        _context.Add(site);
        _context.SaveChanges();
        return RedirectToAction("VersListeSiteProjet");
    }
    
    public IActionResult VersMap(int idsite)
    {
        Site site = _context.Site
            .Include(a => a.Commune)
            .First(a => a.Id == idsite);
        Region region = _context.Region
            .Include(a => a.Province)
            .First(a => a.Id == site.IdRegion);
        string province = region.Province.Nom;
        string commune = site.Commune.Nom;
        ViewBag.province = province;
        ViewBag.commune = commune;
        return View("~/Views/SiteProjet/Map.cshtml");
    }
}