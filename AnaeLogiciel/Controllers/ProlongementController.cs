using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using iTextSharp.awt.geom;
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
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
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
        string messageerreur = "";
        try
        {
            Double.Parse(budget);
        }
        catch (Exception e)
        {
            messageerreur += "- budget invalide -";
        }

        if (messageerreur == "")
        {
            ProlongementBudgetProjet pl = new ProlongementBudgetProjet()
            {
                IdProjet = idprojet,
                Budget = Double.Parse(budget),
                DateAction = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Add(pl);
            _context.SaveChanges();
            return RedirectToAction("VersListeProlongementProjet", new { idprojet = idprojet });   
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionBudgetaire", new { idprojet = idprojet });
        }
    }
    
    public IActionResult CreateTempsProjet(int idprojet, DateOnly datefin)
    {
        ProlongementProjet pl = new ProlongementProjet()
        {
            IdProjet = idprojet,
            DateFin = datefin,
            DateAction = DateOnly.FromDateTime(DateTime.Now)
        };
        _context.Add(pl);
        _context.SaveChanges();
        return RedirectToAction("VersListeProlongementProjet", new { idprojet = idprojet });
    }

    public IActionResult VersListeProlongementOccurenceActivite(int idoccurenceactivite)
    {
        List<ProlongementBudgetOccurenceActivite> liste = _context
            .ProlongementBudgetOccurenceActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewData["listeprolongementbudgetaire"] = liste;
        List<ProlongementOccurenceActivite> listep = _context
            .ProlongementOccurenceActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewData["listeprolongement"] = listep;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/ProlongementOccurenceActivite/Liste.cshtml");
    }

    public IActionResult VersInsertionOccurenceActiviteTemps(int idoccurenceactivite)
    {
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/ProlongementOccurenceActivite/InsertionOccurenceActiviteTemps.cshtml");
    }
    
    public IActionResult VersInsertionOccurenceActiviteBudget(int idoccurenceactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/ProlongementOccurenceActivite/InsertionOccurenceActiviteBudget.cshtml");
    }
    
    public IActionResult CreateOccurenceActiviteTemps(DateOnly datefin, int idoccurenceactivite)
    {
        ProlongementOccurenceActivite pr = new ProlongementOccurenceActivite()
        {
            DateFin = datefin,
            IdOccurenceActivite = idoccurenceactivite,
            DateAction = DateOnly.FromDateTime(DateTime.Now)
        };
        _context.Add(pr);
        _context.SaveChanges();
        return RedirectToAction("VersListeProlongementOccurenceActivite",new {idoccurenceactivite = idoccurenceactivite});
    }
    
    public IActionResult CreateOccurenceActiviteBudget(string budget, int idoccurenceactivite)
    {
        string messageerreur = "";
        try
        {
            Double.Parse(budget);
        }
        catch (Exception e)
        {
            messageerreur += "- budget invalide -";
        }

        if (messageerreur == "")
        {
            ProlongementBudgetOccurenceActivite pr = new ProlongementBudgetOccurenceActivite()
            {
                Budget = Double.Parse(budget),
                IdOccurenceActivite = idoccurenceactivite,
                DateAction = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Add(pr);
            _context.SaveChanges();
            return RedirectToAction("VersListeProlongementOccurenceActivite",new {idoccurenceactivite = idoccurenceactivite});   
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionOccurenceActiviteBudget",
                new { idoccurenceactivite = idoccurenceactivite });
        }
    }
}