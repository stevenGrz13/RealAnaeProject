using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnaeLogiciel.Controllers;

public class OccurenceResultatController : Controller
{
    private readonly ApplicationDbContext _context;

    public OccurenceResultatController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult deleteResultat(int idoccurenceresultat)
    {
        OccurenceResultat or = _context.OccurenceResultat
            .First(a => a.Id == idoccurenceresultat);
        or.IsSupp = true;
        _context.SaveChanges();
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        return RedirectToAction("Details", "Projet", new {idprojet = idprojet});
    }

    public IActionResult VersModif(int idoccurenceresultat)
    {
        OccurenceResultat or = _context.OccurenceResultat
            .First(a => a.Id == idoccurenceresultat);
        ViewData["occurenceresultat"] = or;
        return View("Modification");
    }

    public IActionResult Modification(int idoccurenceresultat, string nom)
    {
        OccurenceResultat or = _context.OccurenceResultat
            .First(a => a.Id == idoccurenceresultat);
        or.NomResultat = nom;
        _context.SaveChanges();
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        return RedirectToAction("Details", "Projet", new {idprojet = idprojet});
    }
}