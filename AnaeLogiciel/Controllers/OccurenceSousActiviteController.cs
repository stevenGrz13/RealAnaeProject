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

    public IActionResult ListeOccurenceSousActivite(int idoccurenceactivite)
    {
        OccurenceActivite o = _context.OccurenceActivite
            .First(a => a.Id == idoccurenceactivite);
        ViewBag.idoccurenceresultat = o.IdOccurenceResultat;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        List<OccurenceSousActivite> listes = _context.OccurenceSousActivite
            .Include(a => a.SousActivite)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        foreach (var v in listes)
        {
            List<OccurenceSousActiviteIndicateur> liste = _context.OccurenceSousActiviteIndicateur
                .Include(a => a.TypeIndicateur)
                .Where(a => a.IdOccurenceSousActivite == v.Id).ToList();
            double[] table = new double[liste.Count];
            for (int i = 0; i < table.Length; i++)
            {   
                var element = _context.VCalculRapportSousActivite
                    .FirstOrDefault(a => a.IdOccurenceSousActivite == v.Id && a.IdIndicateur == liste[i].IdIndicateur);

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
            Console.WriteLine("LENGTH="+table.Length);
            if (Double.IsNaN(moyenne))
            {
                moyenne = 0;
            }
            ViewData["listeoccurenceactiviteindicateur"] = liste;
            OccurenceSousActivite oc = _context.OccurenceSousActivite
                .Include(a => a.SousActivite)
                .First(a => a.Id == v.Id);
            oc.Avancement = moyenne;
            DateOnly datenow = Fonction.Fonction.getDateNow();

            if ((oc.Avancement < 100) && oc.DateFin < datenow)
            {
                oc.Couleur = "text-danger";
            }
            else
            {
                oc.Couleur = "text-success";
            }
            _context.SaveChanges();
        }
        ViewData["listeoccurencesousactivite"] = listes;
        return View("~/Views/OccurenceSousActivite/Liste.cshtml");
    }

    public IActionResult VersInsertionOccurenceSousActivite(int idoccurenceactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"].ToString();
        }
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        ViewData["listesousactivite"] = _context.SousActivite.ToList();
        return View("~/Views/OccurenceSousActivite/Insertion.cshtml");
    }

    public IActionResult Create(int idoccurenceactivite, int idsousactivite, string budget, DateOnly datedebut, DateOnly datefin)
    {
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
                IdSousActivite = idsousactivite,
                Budget = Double.Parse(budget),
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
        ViewData["listesiteoccurencesousactivite"] = _context.SiteSousActivite
            .Include(a => a.Commune)
            .Include(a => a.Region)
            .Include(a => a.District)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite).ToList();
        ViewData["listeoccurencesousactiviteindicateur"] = _context
            .OccurenceSousActiviteIndicateur
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/OccurenceSousActivite/Details.cshtml");
    }

    public IActionResult VersDetailsSiteSousActivite(int idsitesousactivite, int idoccurencesousactivite)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        ViewBag.idsitesousactivite = idsitesousactivite;
        SiteSousActivite st = _context.SiteSousActivite
            .First(a => a.Id == idsitesousactivite);
        ViewBag.idoccurencesousactivite = st.IdOccurenceSousActivite;
        ViewData["listetechnicien"] = _context
            .TechnicienProjet
            .Include(a => a.Technicien)
            .Where(a => a.IdProjet == idprojet).ToList();
        ViewData["listeindicateur"] = _context
            .OccurenceSousActiviteIndicateur
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        ViewData["indicateurtechniciensitesousactivite"] = _context
            .IndicateurTechnicienSiteSousActivite
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdSiteSousActivite == idsitesousactivite)
            .ToList();
        return View("~/Views/SiteSousActivite/Details.cshtml");
    }
    
    public IActionResult RetourVersDetailsOccurenceSousActivite(int idoccurencesousactivite, int idoccurenceactivite)
    {
        return RedirectToAction("VersDetailsOccurenceSousActivite", new { idoccurencesousactivite = idoccurencesousactivite, idoccurenceactivite = idoccurenceactivite });
    }


    public IActionResult RetourVersListeOccurenceSousActivite(int idoccurenceactivite)
    {
        return RedirectToAction("ListeOccurenceSousActivite", new { idoccurenceactivite = idoccurenceactivite });
    }
}