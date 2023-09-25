using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AnaeLogiciel.Controllers;

public class SousActiviteSourceDeVerificationController : Controller
{
    private readonly ApplicationDbContext _context;

    public SousActiviteSourceDeVerificationController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersListe(int idoccurencesousactivite)
    {
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        List<OccurenceSousActiviteSource> liste = _context.OccurenceSousActiviteSource
            .Include(a => a.SourceDeVerification)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        ViewData["listesource"] = liste;
        return View("~/Views/OccurenceSousActiviteSource/Liste.cshtml");
    }

    public IActionResult VersInsertion(int idoccurencesousactivite)
    {
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        List<SourceDeVerification> source = _context.SourceDeVerification
            .ToList();
        ViewData["listeverification"] = source;
        return View("~/Views/OccurenceSousActiviteSource/Insertion.cshtml");
    }

    public IActionResult Create(List<int> idsource, int idoccurencesousactivite)
    {
        foreach (var v in idsource)
        {
            OccurenceSousActiviteSource oa = new OccurenceSousActiviteSource()
            {
                IdOccurenceSousActivite = idoccurencesousactivite,
                IdSource = v
            };
            _context.Add(oa);
        }
        _context.SaveChanges();
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        List<OccurenceSousActiviteSource> liste = _context.OccurenceSousActiviteSource
            .Include(a => a.SourceDeVerification)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        ViewData["listesource"] = liste;
        return View("~/Views/OccurenceSousActiviteSource/Liste.cshtml");
    }
}