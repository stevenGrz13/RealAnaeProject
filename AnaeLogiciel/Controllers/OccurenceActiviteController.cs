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
        ViewBag.idoccurenceresultat = idoccurenceresultat;
        ViewData["listeactivite"] = _context.Activite.ToList();
        return View("~/Views/OccurenceActivite/Create.cshtml");
    }

    public IActionResult Create(int idoccurenceresultat, DateOnly datedebut, DateOnly datefin, string budget, int idactivite)
    {
        OccurenceActivite oa = new OccurenceActivite()
        {
            IdOccurenceResultat = idoccurenceresultat,
            IdActivite = idactivite,
            Budget = Double.Parse(budget),
            DateDebut = datedebut,
            DateFin = datefin
        };
        _context.Add(oa);
        _context.SaveChanges();
        ViewData["listeprojet"] = _context.Projet
            .Include(a => a.Bailleur)
            .ToList();
        return View("~/Views/Projet/Index.cshtml");
    }

    public IActionResult ListeOccurenceActivites(int idoccurenceresultat)
    {
        ViewData["listeoccurenceactivite"] = _context.OccurenceActivite
            .Include(a => a.Activite)
            .Where(a => a.IdOccurenceResultat == idoccurenceresultat).ToList();
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
        
        if (lien != null)
        {
            newmoyenne = 0;
            if (lien.Count > 0)
            {
                foreach (var z in lien)
                {
                    newmoyenne += z.Avancement;
                }

                newmoyenne = newmoyenne / lien.Count;
            }   
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
            .Include(a => a.Activite)
            .First(a => a.Id == idoccurenceactivite);

        if (lien == null)
        {
            oc.Avancement = moyenne;    
        }
        else
        {
            oc.Avancement = (moyenne + newmoyenne) / 2;
        }
        
        _context.SaveChanges();
        ViewData["occurenceactivite"] = oc;
        ViewData["listesiteoccurenceactivite"] = _context.SiteActivite
            .Include(a => a.Commune)
            .Include(a => a.District)
            .Include(a => a.Region)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        return View("~/Views/OccurenceActivite/Details.cshtml");
    }

    public IActionResult VersInsertionIndicateurActivite(int idoccurenceactivite)
    {
        ViewData["listeindicateur"] = _context.TypeIndicateur.ToList();
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/ActiviteIndicateur/Insertion.cshtml");
    }
}