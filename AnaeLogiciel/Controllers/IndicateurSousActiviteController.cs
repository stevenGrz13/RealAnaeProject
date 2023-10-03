using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnaeLogiciel.Controllers;

public class IndicateurSousActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public IndicateurSousActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersInsertionSousActiviteIndicateur(int idoccurencesousactivite)
    {
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        ViewData["listeindicateur"] = _context
            .TypeIndicateur.ToList();
        return View("~/Views/SousActiviteIndicateur/Insertion.cshtml");
    }

    public IActionResult Create(string target, int idindicateur, int idoccurencesousactivite)
    {
        OccurenceSousActiviteIndicateur os = new OccurenceSousActiviteIndicateur()
        {
            IdOccurenceSousActivite = idoccurencesousactivite,
            IdIndicateur = idindicateur,
            Target = Double.Parse(target)
        };
        _context.Add(os);
        _context.SaveChanges();
        return RedirectToAction("VersDetailsOccurenceSousActivite", "OccurenceSousActivite",
            new { idoccurencesousactivite = idoccurencesousactivite });
    }
}