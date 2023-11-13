using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Humanizer;
using iTextSharp.awt.geom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class TravailTechnicienSousActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public TravailTechnicienSousActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult AttributionTravailSousActivite(int idoccurencesousactiviteindicateur)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        ViewBag.idoccurencesousactiviteindicateur = idoccurencesousactiviteindicateur;
        List<TechnicienProjet> liste = _context
            .TechnicienProjet
            .Include(a => a.Technicien)
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        ViewData["liste"] = liste;
        return View("~/Views/TravailTechnicienSousActivite/Insertion.cshtml");
    }

    public IActionResult Create(int idoccurencesousactiviteindicateur, int idtechnicien, string target)
    {
        int idoccurencesousactivite = _context.OccurenceSousActiviteIndicateur
            .First(a => a.Id == idoccurencesousactiviteindicateur)
            .IdOccurenceSousActivite;
        int idoccurenceactivite = _context.OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite)
            .IdOccurenceActivite;
        string messageerreur = "";
        try
        {
            Double.Parse(target);
        }
        catch (Exception e)
        {
            messageerreur += "- target invalide -";
        }

        int idindicateur = _context.OccurenceSousActiviteIndicateur
            .First(a => a.Id == idoccurencesousactiviteindicateur)
            .IdIndicateurSousActivite;
        
        if (messageerreur == "")
        {
            TargetTechnicienIndicateurSousActivite tg = new TargetTechnicienIndicateurSousActivite()
            {
                IdIndicateurSousActivite = idindicateur,
                IdTechnicien = idtechnicien,
                Target = Double.Parse(target)
            };
            _context.Add(tg);
            _context.SaveChanges();
            return RedirectToAction("VersDetailsOccurenceSousActivite", "OccurenceSousActivite", new { idoccurenceactivite = idoccurenceactivite , idoccurencesousactivite = idoccurencesousactivite});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("AttributionTravailSousActivite", new { idoccurencesousactiviteindicateur = idoccurencesousactiviteindicateur });
        }
    }

    public IActionResult versListeActionSousActivite(int idindicateur, int idtechnicien, int idindicateursousactivite)
    {
        List<RapportIndicateurSousActivite> liste = _context.RapportIndicateurSousActivite
            .Include(a => a.Technicien)
            .Include(a => a.IndicateurSousActivite)
            .Where(a => a.IdIndicateurSousActivite == idindicateur && a.IdTechnicien == idtechnicien)
            .ToList();
        ViewData["liste"] = liste;
        ViewBag.idindicateursousactivite = idindicateursousactivite;
        Technicien technicien = _context.Technicien.First(a => a.Id == idtechnicien);
        ViewBag.nomtechnicien = technicien.Email;
        IndicateurSousActivite indicateur = _context.IndicateurSousActivite
            .First(a => a.Id == idindicateur);
        ViewBag.nomindicateur = indicateur.NomIndicateur;
        return View("Liste");
    }
    
    public IActionResult versCreateActionSousActivite(int idtechnicien, int idindicateur, int idindicateursousactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        ViewBag.idtechnicien = idtechnicien;
        ViewBag.idindicateur = idindicateur;
        ViewBag.idindicateursousactivite = idindicateursousactivite;
        return View("ActionSousActiviteTechnicien");
    }

    public IActionResult CreateWithDate(int idtechnicien, int idindicateur, string quantiteeffectue, DateOnly datedebut, DateOnly datefin, int idindicateursousactivite)
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
            RapportIndicateurSousActivite ria = new RapportIndicateurSousActivite()
            {
                IdTechnicien = idtechnicien,
                IdIndicateurSousActivite = idindicateur,
                QuantiteEffectue = Double.Parse(quantiteeffectue),
                DateDebut = datedebut,
                DateFin = datefin
            };
            _context.Add(ria);
            _context.SaveChanges();
            Fonction.Fonction.Action(idprojet, _context);
            return RedirectToAction("versListeActionSousActivite",
                new { idindicateur = idindicateur, idtechnicien = idtechnicien, idindicateursousactivite = idindicateursousactivite });
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("versCreateActionSousActivite",
                new { idindicateur = idindicateur, idtechnicien = idtechnicien, idindicateursousactivite = idindicateursousactivite });
        }
    }
    
    public IActionResult VersModif(int idtargettechnicien, int idoccurencesousactiviteindicateur)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        TargetTechnicienIndicateurSousActivite target = _context.TargetTechnicienIndicateurSousActivite
            .First(a => a.Id == idtargettechnicien);
        ViewData["target"] = target;
        ViewBag.idoccurencesousactiviteindicateur = idoccurencesousactiviteindicateur;
        return View("Modification");
    }

    public IActionResult Modification(string target, int idtargettechnicien, int idoccurencesousactiviteindicateur)
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
            TargetTechnicienIndicateurSousActivite targettechnicien = _context
                .TargetTechnicienIndicateurSousActivite
                .First(a => a.Id == idtargettechnicien);
            targettechnicien.Target = Double.Parse(target);
            _context.SaveChanges();
            Fonction.Fonction.Action(idprojet,_context);
            return RedirectToAction("DetailsIndicateurSousActivite","IndicateurSousActivite", new {idoccurencesousactiviteindicateur = idoccurencesousactiviteindicateur});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersModif", new {idtargettechnicien = idtargettechnicien, idoccurencesousactiviteindicateur = idoccurencesousactiviteindicateur});
        }
    }

    public IActionResult VersModifAction(int idaction, int idindicateursousactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        RapportIndicateurSousActivite rapport = _context.RapportIndicateurSousActivite
            .First(a => a.Id == idaction);
        ViewData["action"] = rapport;
        ViewBag.idindicateursousactivite = idindicateursousactivite;
        return View("ModificationAction");
    }

    public IActionResult ModificationAction(int idaction, string quantiteeffectue, DateOnly datedebut, DateOnly datefin, int idindicateursousactivite)
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
            RapportIndicateurSousActivite risa = _context.RapportIndicateurSousActivite
                .First(a => a.Id == idaction);
            risa.QuantiteEffectue = Double.Parse(quantiteeffectue);
            risa.DateDebut = datedebut;
            risa.DateFin = datefin;
            _context.SaveChanges();  
            Fonction.Fonction.Action(idprojet,_context);
            return RedirectToAction("versListeActionSousActivite", new {idindicateursousactivite = idindicateursousactivite, idindicateur = risa.IndicateurSousActivite, idtechnicien = risa.IdTechnicien });
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersModifAction", new { idaction = idaction, idindicateursousactivite = idindicateursousactivite });
        }
    }
}