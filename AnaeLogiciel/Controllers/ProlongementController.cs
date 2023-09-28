using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnaeLogiciel.Controllers;

public class ProlongementController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProlongementController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersListeProlongementProjet(int idprojet)
    {
        ViewBag.idprojet = idprojet;
        List<ProlongementBudgetProjet> listebudget = _context
            .ProlongementBudgetProjet
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        List<ProlongementProjet> listep = _context
            .ProlongementProjet
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        ViewData["listeprolongement"] = listep;
        ViewData["listeprolongementbudget"] = listebudget;
        return View("ListeProlongementProjet");
    }

    public IActionResult VersInsertionBudgetaire(int idprojet)
    {
        ViewBag.idprojet = idprojet;
        return View("InsertionBudgetProjet");
    }

    public IActionResult VersInsertionTemporelle(int idprojet)
    {
        ViewBag.idprojet = idprojet;
        return View("InsertionProjetTemporelle");
    }

    public IActionResult CreateBudgetProjet(int idprojet, string budget)
    {
        ProlongementBudgetProjet pl = new ProlongementBudgetProjet()
        {
            IdProjet = idprojet,
            Budget = Double.Parse(budget)
        };
        _context.Add(pl);
        _context.SaveChanges();
        return RedirectToAction("VersListeProlongementProjet", new { idprojet = idprojet });
    }
    
    public IActionResult CreateTempsProjet(int idprojet, DateOnly datefin)
    {
        ProlongementProjet pl = new ProlongementProjet()
        {
            IdProjet = idprojet,
            DateFin = datefin
        };
        _context.Add(pl);
        _context.SaveChanges();
        return RedirectToAction("VersListeProlongementProjet", new { idprojet = idprojet });
    }
}