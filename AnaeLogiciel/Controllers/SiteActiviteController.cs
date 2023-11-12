using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnaeLogiciel.Controllers;

public class SiteActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public SiteActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersEtatSiteActivite(int idoccurenceactivite)
    {
        List<SiteActivite> liste = _context.SiteActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewData["listesite"] = liste;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("Liste");
    }

    public IActionResult Create(string libelle, int idoccurenceactivite)
    {
        SiteActivite st = new SiteActivite()
        {
            IdOccurenceActivite = idoccurenceactivite,
            Libelle = libelle
        };
        _context.Add(st);
        _context.SaveChanges();
        return RedirectToAction("VersEtatSiteActivite", new { idoccurenceactivite = idoccurenceactivite });
    }
}