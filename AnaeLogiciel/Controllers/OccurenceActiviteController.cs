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
        ViewData["listeactivite"] = _context.Activite
                .OrderByDescending(a => a.Id)
                .ToList();
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

        if (messageerreur == "")
        {
            OccurenceActivite oa = new OccurenceActivite()
            {
                IdOccurenceResultat = idoccurenceresultat,
                Budget = Double.Parse(budget),
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
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        double[] table = new double[liste.Count];
        for (int i = 0; i < table.Length; i++)
        {   
            var element = _context.VCalculRapportActivite
                .FirstOrDefault(a => a.IdOccurenceActivite == idoccurenceactivite && a.IdIndicateur == liste[i].IdIndicateur);

            if (element != null)
            {
                double somme = element.Somme;
                table[i] = (somme * 100) / liste[i].Target;
            }
            else
            {
                table[i] = 0;
            } 
        }

        double moyenne = 0;
        for (int i = 0; i < table.Length; i++)
        {
            moyenne += table[i];
        }

        moyenne = moyenne / table.Length;

        List<VLienActiviteSousActivite> lien = _context.VLienActiviteSousActivite
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();

        double newmoyenne = 0;
        
        if (lien.Count > 0)
        {
            foreach (var z in lien)
            {
                newmoyenne += z.Avancement;
            }
            newmoyenne = newmoyenne / lien.Count;
        }
        else
        {
            newmoyenne = 0;
        }
        
        if (Double.IsNaN(moyenne))
        {
            moyenne = 0;
        }

        if (Double.IsNaN(newmoyenne))
        {
            newmoyenne = 0;
        }
        
        ViewData["listeoccurenceactiviteindicateur"] = liste;
        OccurenceActivite oc = _context.OccurenceActivite
            .First(a => a.Id == idoccurenceactivite);

        if (lien.Count == 0)
        {
            oc.Avancement = moyenne;    
        }
        else
        {
            oc.Avancement = (moyenne + newmoyenne) / 2;
        }
        
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
        ViewData["listesiteoccurenceactivite"] = _context.SiteActivite
            .Include(a => a.Commune)
            .Include(a => a.District)
            .Include(a => a.Region)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        ViewData["listepartieprenante"] = _context.PartiePrenanteOccurenceActivite
            .Include(a => a.PartiePrenante)
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
        ViewData["listepartieprenante"] = _context.Partieprenante.ToList();
        return View("~/Views/OccurenceActivite/InsertionPartiePrenante.cshtml");
    }

    public IActionResult InsertionPartiePrenante(List<int> idpartieprenante, int idoccurenceactivite)
    {
        foreach (var v in idpartieprenante)
        {
            PartiePrenanteOccurenceActivite pr = new PartiePrenanteOccurenceActivite()
            {
                IdOccurenceActivite = idoccurenceactivite,
                IdPartiePrenante = v
            };
            _context.Add(pr);
        }

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
}