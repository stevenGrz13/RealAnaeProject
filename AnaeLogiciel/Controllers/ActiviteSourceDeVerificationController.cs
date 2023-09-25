using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AnaeLogiciel.Controllers;

public class ActiviteSourceDeVerificationController : Controller
{
    private readonly ApplicationDbContext _context;

    public ActiviteSourceDeVerificationController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersListe(int idoccurenceactivite)
    {
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        List<OccurenceActiviteSource> liste = _context.OccurenceActiviteSource
            .Include(a => a.SourceDeVerification)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewData["listesource"] = liste;
        return View("~/Views/OccurenceActiviteSource/Liste.cshtml");
    }

    public IActionResult VersInsertion(int idoccurenceactivite)
    {
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        List<SourceDeVerification> source = _context.SourceDeVerification
            .ToList();
        ViewData["listeverification"] = source;
        return View("~/Views/OccurenceActiviteSource/Insertion.cshtml");
    }

    public IActionResult Create(List<int> idsource, int idoccurenceactivite)
    {
        foreach (var v in idsource)
        {
            OccurenceActiviteSource oa = new OccurenceActiviteSource()
            {
                IdOccurenceActivite = idoccurenceactivite,
                IdSource = v
            };
            _context.Add(oa);
        }
        _context.SaveChanges();
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        List<OccurenceActiviteSource> liste = _context.OccurenceActiviteSource
            .Include(a => a.SourceDeVerification)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewData["listesource"] = liste;
        return View("~/Views/OccurenceActiviteSource/Liste.cshtml");
    }
}