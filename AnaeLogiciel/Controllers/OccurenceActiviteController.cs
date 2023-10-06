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
        ViewData["listeactivite"] = _context.Activite.ToList();
        return View("~/Views/OccurenceActivite/Create.cshtml");
    }

    public IActionResult Create(int idoccurenceresultat, DateOnly datedebut, DateOnly datefin, string budget, int idactivite)
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
            return RedirectToAction("ListeOccurenceActivites", new {idoccurenceresultat = idoccurenceresultat});
        }
        else
        {
            ViewBag.messageerreur = messageerreur;
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersInsertionOccurenceActivite", new {idoccurenceresultat = idoccurenceresultat});
        }
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

        Console.WriteLine("------------------------------------");
        Console.WriteLine("taille tableau="+lien.Count);
        Console.WriteLine("------------------------------------");
        
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
            .Include(a => a.Activite)
            .First(a => a.Id == idoccurenceactivite);

        if (lien.Count == 0)
        {
            Console.WriteLine("null ilay izy");
            oc.Avancement = moyenne;    
        }
        else
        {
            Console.WriteLine("tsy null ilay izy");
            Console.WriteLine("avancement avant="+oc.Avancement);
            oc.Avancement = (moyenne + newmoyenne) / 2;
            Console.WriteLine("avancement apres="+oc.Avancement);
        }
        
        DateOnly datenow = Fonction.Fonction.getDateNow();

        if ((oc.Avancement < 100) && oc.DateFin < datenow)
        {
            oc.Couleur = "text-danger";
        }
        else
        {
            oc.Couleur = "text-success";
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
        ViewData["listeindicateur"] = _context.TypeIndicateur.ToList();
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
}