using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class IndicateurActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public IndicateurActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult VersInsertionActiviteIndicateur(int idoccurenceactivite)
    {
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        ViewData["listeindicateur"] = _context
            .TypeIndicateur.ToList();
        return View("~/Views/ActiviteIndicateur/Insertion.cshtml");
    }

    public IActionResult Create(string target, int idindicateur, int idoccurenceactivite)
    {
        OccurenceActiviteIndicateur os = new OccurenceActiviteIndicateur()
        {
            IdOccurenceActivite = idoccurenceactivite,
            IdIndicateur = idindicateur,
            Target = Double.Parse(target)
        };
        _context.Add(os);
        _context.SaveChanges();
        OccurenceActivite oc = _context.OccurenceActivite
            .Include(a => a.Activite)
            .First(a => a.Id == idoccurenceactivite);
        List<OccurenceActiviteIndicateur> liste = _context.OccurenceActiviteIndicateur
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        ViewData["listeoccurenceactiviteindicateur"] = liste;
        ViewData["occurenceactivite"] = oc;
        ViewData["listesiteoccurenceactivite"] = _context.SiteActivite
            .Include(a => a.Commune)
            .Include(a => a.Region)
            .Include(a => a.District)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        return RedirectToAction("Details", "OccurenceActivite", new { idoccurenceactivite = idoccurenceactivite });
    }
}