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
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        ViewData["listeindicateur"] = _context
            .TypeIndicateur
            .OrderByDescending(a => a.Id)
            .ToList();
        return View("~/Views/ActiviteIndicateur/Insertion.cshtml");
    }

    public IActionResult Create(string target, int idindicateur, int idoccurenceactivite)
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
            OccurenceActiviteIndicateur os = new OccurenceActiviteIndicateur()
            {
                IdOccurenceActivite = idoccurenceactivite,
                IdIndicateur = idindicateur,
                Target = Double.Parse(target)
            };
            _context.Add(os);
            _context.SaveChanges();
            return RedirectToAction("Details", "OccurenceActivite", new { idoccurenceactivite = idoccurenceactivite });   
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionActiviteIndicateur", new {idoccurenceactivite = idoccurenceactivite});
        }
    }
}