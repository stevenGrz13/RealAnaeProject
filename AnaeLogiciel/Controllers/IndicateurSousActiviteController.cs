using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using iTextSharp.awt.geom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class IndicateurSousActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public IndicateurSousActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IActionResult VersInsertionBaseSousActiviteIndicateur(int idoccurencesousactivite, int idoccurenceactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        List<IndicateurSousActivite> liste = _context.IndicateurSousActivite
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        ViewData["liste"] = liste;
        return View("~/Views/SousActiviteIndicateur/Insertion.cshtml");
    }

    public IActionResult versCreateBase(int idoccurencesousactivite, int idoccurenceactivite)
    {
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        return View("~/Views/SousActiviteIndicateur/CreateBase.cshtml");
    }
    
    public IActionResult CreateBase(int idoccurenceactivite, int idoccurencesousactivite, string nomindicateur)
    {
        IndicateurSousActivite ia = new IndicateurSousActivite()
        {
            IdOccurenceSousActivite = idoccurencesousactivite,
            NomIndicateur = nomindicateur
        };
        _context.Add(ia);
        _context.SaveChanges();
        return RedirectToAction("VersDetailsOccurenceSousActivite","OccurenceSousActivite", new {idoccurenceactivite = idoccurenceactivite, idoccurencesousactivite = idoccurencesousactivite});
    }

    public IActionResult Create(int idoccurenceactivite, int idoccurencesousactivite, int idindicateur, string target)
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
            List<OccurenceSousActiviteIndicateur> last = _context.OccurenceSousActiviteIndicateur
                .Where(a =>
                    a.IdIndicateurSousActivite == idindicateur && a.IdOccurenceSousActivite == idoccurencesousactivite)
                    .ToList();
            if (last.Count > 0)
            {
                last[0].Target += Double.Parse(target);
            }
            else
            {
                OccurenceSousActiviteIndicateur oi = new OccurenceSousActiviteIndicateur()
                {
                    IdOccurenceSousActivite = idoccurencesousactivite,
                    IdIndicateurSousActivite = idindicateur,
                    Target = Double.Parse(target)
                };
                _context.Add(oi);
            }
            _context.SaveChanges();
            return RedirectToAction("VersDetailsOccurenceSousActivite","OccurenceSousActivite", new { idoccurenceactivite = idoccurenceactivite, idoccurencesousactivite = idoccurencesousactivite});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionBaseSousActiviteIndicateur", "IndicateurSousActivite",
                new { idoccurenceactivite = idoccurenceactivite, idoccurencesousactivite = idoccurencesousactivite });
        }
    }

    public IActionResult DetailsIndicateurSousActivite(int idoccurencesousactiviteindicateur)
    {
        OccurenceSousActiviteIndicateur oai = _context
            .OccurenceSousActiviteIndicateur
            .Include(a => a.IndicateurSousActivite)
            .First(a => a.Id == idoccurencesousactiviteindicateur);
        List<TargetTechnicienIndicateurSousActivite> liste = _context.TargetTechnicienIndicateurSousActivite
            .Include(a => a.Technicien)
            .Where(a => a.IdIndicateurSousActivite == oai.IdIndicateurSousActivite)
            .ToList();
        ViewData["indicateur"] = oai;
        ViewData["liste"] = liste;
        ViewBag.idoccurencesousactivite = oai.IdOccurenceSousActivite;
        ViewBag.idoccurenceactivite = _context.OccurenceSousActivite
            .First(a => a.Id == oai.IdOccurenceSousActivite).IdOccurenceActivite;
        ViewBag.idindicateursousactivite = oai.Id;
        return View("~/Views/SousActiviteIndicateur/Details.cshtml");
    }

    public IActionResult VersModif(int idoccurencesousactiviteindicateur)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        OccurenceSousActiviteIndicateur oasi = _context.OccurenceSousActiviteIndicateur
            .Include(a => a.OccurenceSousActivite)
            .First(a => a.Id == idoccurencesousactiviteindicateur);
        ViewData["occurencesousactiviteindicateur"] = oasi;
        return View("~/Views/SousActiviteIndicateur/Modification.cshtml");
    }

    public IActionResult Modification(int idoccurencesousactiviteindicateur, string target)
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
            OccurenceSousActiviteIndicateur oasi = _context.OccurenceSousActiviteIndicateur
                .First(a => a.Id == idoccurencesousactiviteindicateur);
            OccurenceSousActivite osa = _context.OccurenceSousActivite
                .First(a => a.Id == oasi.IdOccurenceSousActivite);
            oasi.Target = Double.Parse(target);
            _context.SaveChanges();
            return RedirectToAction("VersDetailsOccurenceSousActivite","OccurenceSousActivite", new {idoccurencesousactivite = oasi.IdOccurenceSousActivite, idoccurenceactivite = osa.IdOccurenceActivite});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersModif", new {idoccurencesousactiviteindicateur = idoccurencesousactiviteindicateur});
        }
    }
}