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
        ViewData["occurenceactivite"] = _context.OccurenceActivite
            .Include(a => a.Activite)
            .First(a => a.Id == idoccurenceactivite);
        return View("~/Views/OccurenceActivite/Details.cshtml");
    }
}