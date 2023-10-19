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
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        OccurenceSousActivite os = _context
            .OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite);
        ViewBag.idoccurenceactivite = os.IdOccurenceActivite;
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        ViewData["listeindicateur"] = _context
            .TypeIndicateur
            .OrderByDescending(a => a.Id)
            .ToList();
        return View("~/Views/SousActiviteIndicateur/Insertion.cshtml");
    }

    public IActionResult Create(string target, int idindicateur, int idoccurencesousactivite, int idoccurenceactivite)
    {
        string messageerreur = "";
        try
        {
            Double.Parse(target);
        }
        catch (Exception e)
        {
            messageerreur += "- target invalide -";
        }

        if (messageerreur == "")
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
                new { idoccurencesousactivite = idoccurencesousactivite, idoccurenceactivite = idoccurenceactivite });   
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionSousActiviteIndicateur",
                new { idoccurencesousactivite = idoccurencesousactivite });
        }
    }
}