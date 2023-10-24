using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class HistoriqueController : Controller
{
    private readonly ApplicationDbContext _context;

    public HistoriqueController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult versDeletedActivite(int idoccurenceresultat)
    {
        ViewData["listeactivite"] = _context.OccurenceActivite
            .Where(a => a.IdOccurenceResultat == idoccurenceresultat && a.IsSupp == true)
            .ToList();
        ViewBag.idoccurenceresultat = idoccurenceresultat;
        return View("DeletedActivite");
    }

    public IActionResult RestaurerActivite(int idoccurenceactivite, int idoccurenceresultat)
    {
        OccurenceActivite oa = _context.OccurenceActivite
            .First(a => a.Id == idoccurenceactivite);
        oa.IsSupp = false;
        _context.SaveChanges();
        return RedirectToAction("ListeOccurenceActivites", "OccurenceActivite", new { idoccurenceresultat = idoccurenceresultat });
    }

    public IActionResult versDeletedSousActivite(int idoccurenceactivite)
    {
        ViewData["listesousactivite"] = _context.OccurenceSousActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite && a.IsSupp == true)
            .ToList();
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("DeletedSousActivite");
    }

    public IActionResult RestaurerSousActivite(int idoccurencesousactivite, int idoccurenceactivite)
    {
        OccurenceSousActivite osa = _context.OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite);
        osa.IsSupp = false;
        _context.SaveChanges();
        return RedirectToAction("RetourVersListeOccurenceSousActivite", "OccurenceSousActivite",
            new { idoccurenceactivite = idoccurenceactivite });
    }

    public IActionResult versDeletedResultat()
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        ViewData["listeresultat"] = _context.OccurenceResultat
            .Where(a => a.IdProjet == idprojet && a.IsSupp == true)
            .ToList();
        return View("DeletedResultat");
    }

    public IActionResult RestaurerResultat(int idoccurenceresultat)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        OccurenceResultat or = _context.OccurenceResultat
            .First(a => a.Id == idoccurenceresultat);
        or.IsSupp = false;
        _context.SaveChanges();
        return RedirectToAction("Details", "Projet", new { idprojet = idprojet });
    }
}