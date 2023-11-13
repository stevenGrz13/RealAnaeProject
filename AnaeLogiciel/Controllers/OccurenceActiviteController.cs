using System.Drawing.Printing;
using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AnaeLogiciel.Controllers;

public class OccurenceActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public OccurenceActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersInsertionOccurenceActivite(int idoccurenceresultat)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"].ToString();
        }
        ViewBag.idoccurenceresultat = idoccurenceresultat;
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        ViewBag.idprojet = idprojet;
        return View("~/Views/OccurenceActivite/Create.cshtml");
    }

    public IActionResult Create(int idoccurenceresultat, DateOnly datedebut, DateOnly datefin, string budget, string details)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        Projet p = _context.Projet.First(a => a.Id == idprojet);
        string messageerreur = "";
        try
        {
            Double.Parse(budget);
        }
        catch (Exception e)
        {
            messageerreur += "- montant invalide -";
        }
        
        if (!Fonction.Fonction.SecureDates(p.DateDebutPrevision, p.DateFinPrevision, datedebut, datefin)||!Fonction.Fonction.SecureDate(datedebut, datefin))
        {
            messageerreur += "- dates invalide -";
        }

        double valeurdevise = _context.Projet.First(a => a.Id == idprojet).ValeurDevise;
        
        if (messageerreur == "")
        {
            OccurenceActivite oa = new OccurenceActivite()
            {
                IdOccurenceResultat = idoccurenceresultat,
                Budget = Double.Parse(budget)*valeurdevise,
                Details = details,
                DateDebut = datedebut,
                DateFin = datefin
            };
            _context.Add(oa);
            _context.SaveChanges();
            OccurenceActivite noa = _context.OccurenceActivite
                .First(a => a == oa);
            RealDataOccurenceActivite ro = new RealDataOccurenceActivite()
            {
                IdOccurenceActivite = noa.Id,
                Budget = noa.Budget,
                DateFin = noa.DateFin
            };
            _context.Add(ro);
            _context.SaveChanges();
            return RedirectToAction("ListeOccurenceActivites", new {idoccurenceresultat = idoccurenceresultat});
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionOccurenceActivite", new {idoccurenceresultat = idoccurenceresultat});
        }
    }
    
    public IActionResult ListeOccurenceActivites(int idoccurenceresultat, int? page, string? search)
    {
        IQueryable<OccurenceActivite> query;
        if (search == null)
        {
            query = _context.OccurenceActivite
                .Where(a => a.IdOccurenceResultat == idoccurenceresultat && a.IsSupp == false);
        }
        else
        {
            query = _context.OccurenceActivite
                .Where(a => a.IdOccurenceResultat == idoccurenceresultat && a.Details.Contains(search) && a.IsSupp == false);            
        }
        if (page == null)
        {
            page = 1;
        }
        int pageSize = 3;
        
        int totalItems = query.Count();

        var pagedList = query
            .OrderBy(a => a.Id)
            .Skip((page.GetValueOrDefault() - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new PagedList<OccurenceActivite>
        {
            Items = pagedList,
            TotalItems = totalItems,
            PageNumber = page.GetValueOrDefault(),
            PageSize = pageSize
        };

        ViewData["listeoccurenceactivite"] = model;
        ViewBag.idoccurenceresultat = idoccurenceresultat;
        return View("~/Views/OccurenceActivite/Liste.cshtml");
    }


    public IActionResult Details(int idoccurenceactivite)
    {
        List<OccurenceActiviteIndicateur> liste = _context.OccurenceActiviteIndicateur
            .Include(a => a.IndicateurActivite)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        
        ViewData["listeoccurenceactiviteindicateur"] = liste;
        OccurenceActivite oc = _context.OccurenceActivite
            .First(a => a.Id == idoccurenceactivite);
        
        DateOnly datenow = Fonction.Fonction.getDateNow();

        if ((oc.Avancement != 100) && oc.DateFin < datenow)
        {
            oc.Couleur = "text-danger";
            oc.Message = "En retard";
        }
        else
        {
            oc.Couleur = "text-success";
            oc.Message = "A temps";
        }

        List<ProlongementOccurenceActivite> listeprolongement = _context.ProlongementOccurenceActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();

        List<ProlongementBudgetOccurenceActivite> listeprolongementbudget = _context
            .ProlongementBudgetOccurenceActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();

        double sommeprolongement = 0;
        
        if (listeprolongement.Count > 0)
        {
            oc.DateFin = listeprolongement.LastOrDefault().DateFin;
        }

        if (listeprolongementbudget.Count > 0)
        {
            foreach (var v in listeprolongementbudget)
            {
                sommeprolongement += v.Budget;
            }

            oc.Budget += sommeprolongement;
        }
        
        _context.SaveChanges();
        
        ViewData["occurenceactivite"] = oc;
        ViewData["listepartieprenante"] = _context.PartiePrenanteOccurenceActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        return View("~/Views/OccurenceActivite/Details.cshtml");
    }

    public IActionResult VersInsertionIndicateurActivite(int idoccurenceactivite)
    {
        ViewData["listeindicateur"] = _context.TypeIndicateur
            .OrderByDescending(a => a.Id)
            .ToList();
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/ActiviteIndicateur/Insertion.cshtml");
    }

    public IActionResult VersInsertionPartiePrenante(int idoccurenceactivite)
    {
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        ViewData["listepartieprenante"] = _context.PartiePrenanteOccurenceActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        return View("~/Views/OccurenceActivite/InsertionPartiePrenante.cshtml");
    }

    public IActionResult InsertionPartiePrenante(string partieprenante, int idoccurenceactivite)
    {
        PartiePrenanteOccurenceActivite pr = new PartiePrenanteOccurenceActivite()
        {
            IdOccurenceActivite = idoccurenceactivite,
            PartiePrenante = partieprenante
        };
        _context.Add(pr);
        _context.SaveChanges();
        return RedirectToAction("Details", new { idoccurenceactivite = idoccurenceactivite });
    }

    public IActionResult RetourVersDetailsOccurenceActivite(int idoccurenceactivite)
    {
        return RedirectToAction("Details", new { idoccurenceactivite = idoccurenceactivite });
    }


    public IActionResult RetourVersListeOccurenceActivite(int idoccurenceresultat)
    {
        return RedirectToAction("ListeOccurenceActivites", new { idoccurenceresultat = idoccurenceresultat });
    }

    public IActionResult deleteActivite(int idoccurenceactivite, int idoccurenceresultat)
    {
        OccurenceActivite oa = _context.OccurenceActivite
            .First(a => a.Id == idoccurenceactivite);
        oa.IsSupp = true;
        _context.SaveChanges();
        return RedirectToAction("ListeOccurenceActivites", new { idoccurenceresultat = idoccurenceresultat });
    }

    public IActionResult VersModif(int idoccurenceactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        double devise = _context.Projet.First(a => a.Id == idprojet)
            .ValeurDevise;
        ViewBag.devise = devise;
        OccurenceActivite oa = _context.OccurenceActivite
            .First(a => a.Id == idoccurenceactivite);
        ViewBag.idoccurenceresultat = oa.IdOccurenceResultat;
        ViewData["occurenceactivite"] = oa;
        return View("Modification");
    }

    public IActionResult Modifier(int idoccurenceactivite, string details, string budget, DateOnly datedebut, DateOnly datefin)
    {
        string messageerreur = "";
        try
        {
            Double.Parse(budget);
        }
        catch
        {
            messageerreur += "- budget invalide -";
        }
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        double valeurdevise = _context.Projet.First(a => a.Id == idprojet).ValeurDevise;

        if (messageerreur == "")
        {
            OccurenceActivite oa = _context.OccurenceActivite
                .First(a => a.Id == idoccurenceactivite);

            oa.Details = details;
            oa.Budget = Double.Parse(budget) * valeurdevise;
            oa.DateDebut = datedebut;
            oa.DateFin = datefin;

            _context.SaveChanges();

            return RedirectToAction("ListeOccurenceActivites", new { idoccurenceresultat = oa.IdOccurenceResultat });   
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersModif", new {idoccurenceactivite = idoccurenceactivite});
        }
    }
}