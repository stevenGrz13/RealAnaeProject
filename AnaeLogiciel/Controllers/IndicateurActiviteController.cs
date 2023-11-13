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
    
    public IActionResult VersInsertionBaseActiviteIndicateur(int idoccurenceactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        List<IndicateurActivite> liste = _context.IndicateurActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewData["liste"] = liste;
        return View("~/Views/ActiviteIndicateur/Insertion.cshtml");
    }

    public IActionResult versCreateBase(int idoccurenceactivite)
    {
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/ActiviteIndicateur/CreateBase.cshtml");
    }
    
    public IActionResult CreateBase(int idoccurenceactivite, string nomindicateur)
    {
        IndicateurActivite ia = new IndicateurActivite()
        {
            IdOccurenceActivite = idoccurenceactivite,
            NomIndicateur = nomindicateur
        };
        _context.Add(ia);
        _context.SaveChanges();
        return RedirectToAction("Details", "OccurenceActivite", new { idoccurenceactivite = idoccurenceactivite});
    }

    public IActionResult Create(int idoccurenceactivite, int idindicateuractivite, string target)
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
            List<OccurenceActiviteIndicateur> last = _context.OccurenceActiviteIndicateur
                .Where(a => a.IdIndicateurActivite == idindicateuractivite && a.IdOccurenceActivite == idoccurenceactivite)
                            .ToList();
            if (last.Count > 0)
            {
                last[0].Target += Double.Parse(target);
            }
            else
            {
                OccurenceActiviteIndicateur oi = new OccurenceActiviteIndicateur()
                {
                    IdOccurenceActivite = idoccurenceactivite,
                    IdIndicateurActivite = idindicateuractivite,
                    Target = Double.Parse(target)
                };
                _context.Add(oi);   
            }
            _context.SaveChanges();
            return RedirectToAction("Details", "OccurenceActivite", new {idoccurenceactivite = idoccurenceactivite});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionBaseActiviteIndicateur", "IndicateurActivite", new {idoccurenceactivite = idoccurenceactivite});
        }
    }

    public IActionResult DetailsIndicateurActivite(int idoccurenceactiviteindicateur)
    {
        OccurenceActiviteIndicateur oai = _context
            .OccurenceActiviteIndicateur
            .Include(a => a.IndicateurActivite)
            .First(a => a.Id == idoccurenceactiviteindicateur);
        List<TargetTechnicienIndicateurActivite> liste = _context.TargetTechnicienIndicateurActivite
            .Include(a => a.Technicien)
            .Where(a => a.IdIndicateurActivite == oai.IdIndicateurActivite)
            .ToList();
        ViewData["indicateur"] = oai;
        ViewData["liste"] = liste;
        return View("~/Views/ActiviteIndicateur/Details.cshtml");
    }

    public IActionResult VersModif(int idoccurenceactiviteindicateur)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        OccurenceActiviteIndicateur oai = _context.OccurenceActiviteIndicateur
            .First(a => a.Id == idoccurenceactiviteindicateur);
        ViewData["occurenceactiviteindicateur"] = oai;
        return View("~/Views/ActiviteIndicateur/Modification.cshtml");
    }

    public IActionResult Modification(int idoccurenceactiviteindicateur, string target)
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
            OccurenceActiviteIndicateur oai = _context.OccurenceActiviteIndicateur
                .First(a => a.Id == idoccurenceactiviteindicateur);
            oai.Target = Double.Parse(target);
            _context.SaveChanges();
            Fonction.Fonction.Action(idprojet,_context);
            return RedirectToAction("Details","OccurenceActivite", new {idoccurenceactivite = oai.IdOccurenceActivite});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersModif", new { idoccurenceactiviteindicateur =idoccurenceactiviteindicateur});
        }
    }
}