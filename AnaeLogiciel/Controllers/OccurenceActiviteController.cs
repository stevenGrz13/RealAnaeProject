using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AnaeLogiciel.Controllers;

public class OccurenceActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public OccurenceActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersInsertionOccurenceActivite(int idoccurenceresultat)
    {
        ViewBag.idoccurenceresultat = idoccurenceresultat;
        ViewData["listeactivite"] = _context.Activite.ToList();
        return View("~/Views/OccurenceActivite/Create.cshtml");
    }

    public IActionResult Create(int idoccurenceresultat, DateOnly datedebut, DateOnly datefin, string budget, int idactivite)
    {
        OccurenceActivite oa = new OccurenceActivite()
        {
            IdOccurenceResultat = idoccurenceresultat,
            IdActivite = idactivite,
            Budget = Double.Parse(budget),
            DateDebut = datedebut,
            DateFin = datefin
        };
        _context.Add(oa);
        _context.SaveChanges();
        ViewData["listeprojet"] = _context.Projet
            .Include(a => a.Bailleur)
            .ToList();
        return View("~/Views/Projet/Index.cshtml");
    }

    public IActionResult ListeOccurenceActivites(int idoccurenceresultat)
    {
        ViewData["listeoccurenceactivite"] = _context.OccurenceActivite
            .Include(a => a.Activite)
            .Where(a => a.IdOccurenceResultat == idoccurenceresultat).ToList();
        return View("~/Views/OccurenceActivite/Liste.cshtml");
    }

    public IActionResult Details(int idoccurenceactivite)
    {
        List<OccurenceActiviteIndicateur> liste = _context.OccurenceActiviteIndicateur
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        ViewData["listeoccurenceactiviteindicateur"] = liste;
        ViewData["occurenceactivite"] = _context.OccurenceActivite
            .Include(a => a.Activite)
            .FirstOrDefault(a => a.Id == idoccurenceactivite);
        ViewData["listesiteoccurenceactivite"] = _context.SiteActivite
            .Include(a => a.Commune)
            .Include(a => a.District)
            .Include(a => a.Region)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        return View("~/Views/OccurenceActivite/Details.cshtml");
    }

    public IActionResult VersInsertionIndicateurActivite(int idoccurenceactivite)
    {
        ViewData["listeindicateur"] = _context.TypeIndicateur.ToList();
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/ActiviteIndicateur/Insertion.cshtml");
    }
}