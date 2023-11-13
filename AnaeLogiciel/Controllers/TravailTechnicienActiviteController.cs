using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class TravailTechnicienActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public TravailTechnicienActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult AttributionTravailActivite(int idoccurenceactiviteindicateur)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        ViewBag.idoccurenceactiviteindicateur = idoccurenceactiviteindicateur;
        List<TechnicienProjet> liste = _context
            .TechnicienProjet
            .Include(a => a.Technicien)
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        ViewData["liste"] = liste;
        return View("~/Views/TravailTechnicienActivite/Insertion.cshtml");
    }

    public IActionResult Create(int idoccurenceactiviteindicateur, int idtechnicien, string target)
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
        int idindicateur = _context.OccurenceActiviteIndicateur
            .First(a => a.Id == idoccurenceactiviteindicateur)
            .IdIndicateurActivite;
        if (messageerreur == "")
        {
            TargetTechnicienIndicateurActivite tg = new TargetTechnicienIndicateurActivite()
            {
                IdIndicateurActivite = idindicateur,
                IdTechnicien = idtechnicien,
                Target = Double.Parse(target)
            };
            _context.Add(tg);
            _context.SaveChanges();   
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("AttributionTravailActivite", new { idoccurenceactiviteindicateur = idoccurenceactiviteindicateur });
        }
        return RedirectToAction("DetailsIndicateurActivite", "IndicateurActivite", new { idoccurenceactiviteindicateur = idoccurenceactiviteindicateur});
    }

    public IActionResult versListeActionActivite(int idindicateur, int idtechnicien, int idoccurenceactiviteindicateur)
    {
        List<RapportIndicateurActivite> liste = _context.RapportIndicateurActivite
            .Include(a => a.IndicateurActivite)
            .Where(a => a.IdIndicateurActivite == idindicateur && a.IdTechnicien == idtechnicien)
            .ToList();
        ViewData["liste"] = liste;
        ViewBag.idoccurenceactiviteindicateur = idoccurenceactiviteindicateur;
        Technicien technicien = _context.Technicien
            .First(a => a.Id == idtechnicien);
        IndicateurActivite indicateur = _context.IndicateurActivite
            .First(a => a.Id == idindicateur);
        ViewBag.nomtechnicien = technicien.Email;
        ViewBag.nomindicateur = indicateur.NomIndicateur;
        return View("Liste");
    }
    
    public IActionResult versCreateActionActivite(int idtechnicien, int idindicateur, int idoccurenceactiviteindicateur)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        ViewBag.idtechnicien = idtechnicien;
        ViewBag.idindicateur = idindicateur;
        ViewBag.idoccurenceactiviteindicateur = idoccurenceactiviteindicateur;
        return View("ActionActiviteTechnicien");
    }

    public IActionResult CreateWithDate(int idtechnicien, int idindicateur, string quantiteeffectue, DateOnly datedebut, DateOnly datefin, int idoccurenceactiviteindicateur)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        string messageerreur = "";
        try
        {
            Double.Parse(quantiteeffectue);
        }
        catch (Exception e)
        {
            messageerreur += "- quantite invalide -";
        }

        if (messageerreur == "")
        {
            RapportIndicateurActivite ria = new RapportIndicateurActivite()
            {
                IdTechnicien = idtechnicien,
                IdIndicateurActivite = idindicateur,
                QuantiteEffectue = Double.Parse(quantiteeffectue),
                DateDebut = datedebut,
                DateFin = datefin
            };
            _context.Add(ria);
            _context.SaveChanges();
            Fonction.Fonction.Action(idprojet, _context);
            return RedirectToAction("versListeActionActivite",
                new { idindicateur = idindicateur, idtechnicien = idtechnicien });
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("versCreateActionActivite",
                new { idindicateur = idindicateur, idtechnicien = idtechnicien, idoccurenceactiviteindicateur = idoccurenceactiviteindicateur });
        }
    }

    public IActionResult VersModif(int idtargettechnicienindicateur, int idoccurenceactiviteindicateur)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        TargetTechnicienIndicateurActivite tti = _context.TargetTechnicienIndicateurActivite
            .Include(a => a.IndicateurActivite)
            .Include(a => a.Technicien)
            .First(a => a.Id == idtargettechnicienindicateur);
        ViewData["targettechnicienindicateur"] = tti;
        ViewBag.idoccurenceactiviteindicateur = idoccurenceactiviteindicateur;
        return View("Modification");
    }
    
    public IActionResult Modifier(int idtechnicientarget, string target, int idoccurenceactiviteindicateur)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
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
            TargetTechnicienIndicateurActivite tti = _context.TargetTechnicienIndicateurActivite
                .First(a => a.Id == idtechnicientarget);
            tti.Target = Double.Parse(target);
            _context.SaveChanges();
            Fonction.Fonction.Action(idprojet,_context);
            return RedirectToAction("DetailsIndicateurActivite","IndicateurActivite", new {idoccurenceactiviteindicateur = idoccurenceactiviteindicateur});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersModif", new { idtargettechnicienindicateur = idtechnicientarget, idoccurenceactiviteindicateur = idoccurenceactiviteindicateur});
        }
    }

    public IActionResult VersModifAction(int idaction, int idoccurenceactiviteindicateur)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        RapportIndicateurActivite action = _context.RapportIndicateurActivite
            .First(a => a.Id == idaction);
        ViewData["action"] = action;
        ViewBag.idoccurenceactiviteindicateur = idoccurenceactiviteindicateur;
        return View("ModificationAction");
    }

    public IActionResult ModificationAction(int idaction, string quantiteeffectue, DateOnly datedebut, DateOnly datefin, int idoccurenceactiviteindicateur)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        string messageerreur = "";
        try
        {
            Double.Parse(quantiteeffectue);
        }
        catch (Exception e)
        {
            messageerreur += "- quantite invalide -";
        }

        if (messageerreur == "")
        {
            RapportIndicateurActivite action = _context.RapportIndicateurActivite
                .First(a => a.Id == idaction);
            action.QuantiteEffectue = Double.Parse(quantiteeffectue);
            action.DateDebut = datedebut;
            action.DateFin = datefin;
            _context.SaveChanges();
            Fonction.Fonction.Action(idprojet,_context);
            return RedirectToAction("versListeActionActivite", new {idindicateur = action.IdIndicateurActivite, idtechnicien = action.IdTechnicien, idoccurenceactiviteindicateur = idoccurenceactiviteindicateur});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersModifAction", new {idaction = idaction, idoccurenceactiviteindicateur = idoccurenceactiviteindicateur});
        }
    }
}