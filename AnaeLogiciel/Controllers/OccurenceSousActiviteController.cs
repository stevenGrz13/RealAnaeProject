using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class OccurenceSousActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public OccurenceSousActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult ListeOccurenceSousActivite(int idoccurenceactivite, int? page, string search)
    {
        if (page == null)
        {
            page = 1;
        }
        
        OccurenceActivite o = _context.OccurenceActivite
            .First(a => a.Id == idoccurenceactivite);
        ViewBag.idoccurenceresultat = o.IdOccurenceResultat;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        var listes = _context.OccurenceSousActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite && a.IsSupp == false)
            .ToList();

        int totalItems = listes.Count();

        int pageSize = 3;
        
        var pagedList = listes
            .OrderBy(a => a.Id)
            .Skip((page.GetValueOrDefault() - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new PagedList<OccurenceSousActivite>
        {
            Items = pagedList,
            TotalItems = totalItems,
            PageNumber = page.GetValueOrDefault(),
            PageSize = pageSize
        };
        ViewData["listeoccurencesousactivite"] = model;
        return View("~/Views/OccurenceSousActivite/Liste.cshtml");
    }

    public IActionResult VersInsertionOccurenceSousActivite(int idoccurenceactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"].ToString();
        }
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/OccurenceSousActivite/Insertion.cshtml");
    }

    public IActionResult Create(int idoccurenceactivite, string budget, DateOnly datedebut, DateOnly datefin, string details)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        double valeurdevise = _context.Projet.First(a => a.Id == idprojet).ValeurDevise;
        string messageerreur = "";
        OccurenceActivite oc = _context.OccurenceActivite
            .First(a => a.Id == idoccurenceactivite);
        try
        {
            Double.Parse(budget);
        }
        catch (Exception e)
        {
            messageerreur += "- montant invalide -";
        }
        
        if((!Fonction.Fonction.SecureDates(oc.DateDebut,oc.DateFin,datedebut,datefin))||(!Fonction.Fonction.SecureDate(datedebut,datefin)))
        {
            messageerreur += "- dates invalide -";
        }

        if (messageerreur == "")
        {
            OccurenceSousActivite osc = new OccurenceSousActivite()
            {
                IdOccurenceActivite = idoccurenceactivite,
                Budget = Double.Parse(budget)*valeurdevise,
                Details = details,
                DateDebut = datedebut,
                DateFin = datefin
            };
            _context.Add(osc);
            _context.SaveChanges();
            return RedirectToAction("ListeOccurenceSousActivite", new {idoccurenceactivite = idoccurenceactivite});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionOccurenceSousActivite", new {idoccurenceactivite = idoccurenceactivite});
        }
    }

    public IActionResult VersDetailsOccurenceSousActivite(int idoccurencesousactivite, int idoccurenceactivite)
    {
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        OccurenceSousActivite osc = _context
            .OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite);
        if (Fonction.Fonction.getDateNow() > osc.DateFin && osc.Avancement != 100)
        {
            osc.Couleur = "text-danger";
            osc.Message = "En retard";
        }
        else
        {
            osc.Couleur = "text-success";
            osc.Message = "A temps";
        }
        ViewData["osc"] = osc;
        ViewData["liste"] = _context.OccurenceSousActiviteIndicateur
            .Include(a => a.IndicateurSousActivite)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        return View("~/Views/OccurenceSousActivite/Details.cshtml");
    }

    public IActionResult deleteSousActivite(int idoccurencesousactivite)
    {
        OccurenceSousActivite osa = _context.OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite);
        osa.IsSupp = true;
        _context.SaveChanges();
        int idoccurenceactivite = osa.IdOccurenceActivite;
        return RedirectToAction("ListeOccurenceSousActivite", new { idoccurenceactivite = idoccurenceactivite });
    }
    
    public IActionResult RetourVersDetailsOccurenceSousActivite(int idoccurencesousactivite, int idoccurenceactivite)
    {
        return RedirectToAction("VersDetailsOccurenceSousActivite", new { idoccurencesousactivite = idoccurencesousactivite, idoccurenceactivite = idoccurenceactivite });
    }

    public IActionResult RetourVersListeOccurenceSousActivite(int idoccurenceactivite)
    {
        return RedirectToAction("ListeOccurenceSousActivite", new { idoccurenceactivite = idoccurenceactivite });
    }

    public IActionResult VersModif(int idoccurencesousactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        ViewBag.devise = _context.Projet.First(a => a.Id == idprojet).ValeurDevise;
        OccurenceSousActivite osa = _context.OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite);
        ViewData["occurencesousactivite"] = osa;
        return View("Modification");
    }

    public IActionResult Modifier(int idoccurencesousactivite, string details, string budget, DateOnly datedebut, DateOnly datefin)
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
            int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
            double devise = _context.Projet.First(a => a.Id == idprojet).ValeurDevise;
            OccurenceSousActivite osa = _context.OccurenceSousActivite
                .First(a => a.Id == idoccurencesousactivite);
            osa.Details = details;
            osa.Budget = Double.Parse(budget) * devise;
            osa.DateDebut = datedebut;
            osa.DateFin = datefin;
            _context.SaveChanges();
            return RedirectToAction("ListeOccurenceSousActivite", new {idoccurenceactivite = osa.IdOccurenceActivite});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersModif", new {idoccurencesousactivite = idoccurencesousactivite});
        }
    }
}