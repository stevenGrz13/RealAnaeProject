using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class MyController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SmtpConfig _smtpConfig;

    public MyController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, SmtpConfig smtpConfig)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _smtpConfig = smtpConfig;
    }

    public IActionResult VersInsertionIndicateurSansDate(int idoccurence)
    {
        ViewBag.idoccurence = idoccurence;
        List<TypeIndicateur> listeindicateur = _context.TypeIndicateur.ToList();
        ViewData["listeindicateur"] = listeindicateur;
        return View("~/Views/TraitementIndicateur/SansDate/InsertionAssociationIndicateur.cshtml");
    }
    
    public IActionResult VersInsertionIndicateurAvecDate(int idoccurence)
    {
        if (TempData["idoccurence"] != null)
        {
            idoccurence = (int)TempData["idoccurence"];
        }

        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        ViewBag.idoccurence = idoccurence;
        List<AssociationIndicateurActivite> associationIndicateurActivite =
            _context.IndicateurActiviteProjet
                .Include(a => a.OccurenceActivite)
                .Include(a => a.TypeIndicateur)
                .Where(a => a.IdOccurence == idoccurence)
                .ToList();
        ViewData["listeassociation"] = associationIndicateurActivite;
        return View("~/Views/TraitementIndicateur/AvecDate/InsertionAssociationIndicateur.cshtml");
    }
    
    public IActionResult CreateSansDate(int idoccurence,int idtypeindicateur, string quantitedemandee)
    {
        bool exist = _context.IndicateurActiviteProjet.Any(a =>
            a.IdOccurence == idoccurence && a.IdTypeIndicateur == idtypeindicateur);
        OccurenceActivite oa = _context.ActiviteProjet.First(a => a.Id == idoccurence);
        AssociationIndicateurActivite a = new AssociationIndicateurActivite()
        {
            IdOccurence = idoccurence,
            IdTypeIndicateur = idtypeindicateur,
            QuantiteDemande = Double.Parse(quantitedemandee)
        };
        Console.WriteLine("ENY SA TSIAAAAAA= "+exist);
        if (exist)
        {
            AssociationIndicateurActivite associationIndicateurActivite = _context
                .IndicateurActiviteProjet
                .First(a => a.IdTypeIndicateur == idtypeindicateur && a.IdOccurence == idoccurence);
            associationIndicateurActivite.QuantiteDemande += Double.Parse(quantitedemandee);
        }
        else
        {
            _context.Add(a);   
        }
        _context.SaveChanges();
        TempData["idprojet"] = oa.IdProjet;
        return RedirectToAction("RealIndex","OccurenceActivite");
    }
    
    public IActionResult CreateAvecDate(int idoccurence, int idtypeindicateur, string quantiteeffectuee, int idtechnicien, DateOnly dateaction)
    {
        // OccurenceActivite oa = _context.ActiviteProjet.First(a => a.Id == idoccurence);
        // string messageerreur = "";
        // if (oa.DateDebutPrevision > dateaction)
        // {
        //     messageerreur += "- date invalide a l'activite -";
        // }
        // try
        // {
        //     if (Double.Parse(quantiteeffectuee) <= 0)
        //     {
        //         messageerreur += "quantite inferieur ou egal a 0 non acceptee";
        //     }
        // }
        // catch (Exception e)
        // {
        //     messageerreur += "- quantite invalide -";
        // }
        // idtechnicien = 1;
        // if (messageerreur == "")
        // {
        //     string text = "le technicien xx a effectue " + quantiteeffectuee + _context.TypeIndicateur.First(a => a.Id == idtypeindicateur).Nom + " le " + dateaction;
        //     AssociationIndicateurActiviteAvecDate a = new AssociationIndicateurActiviteAvecDate()
        //     {
        //         IdTechnicien = idtechnicien,
        //         IdOccurence = idoccurence,
        //         IdTypeIndicateur = idtypeindicateur,
        //         TargetEffectue = Double.Parse(quantiteeffectuee),
        //         DateAction = dateaction
        //     };
        //     _context.Add(a);
        //     _context.SaveChanges();   
        //     //Fonction.Fonction.EnvoyerEmail(_smtpConfig,"razafimahefasteven130102@gmail.com", "travisjamesmdg7713@gmail.com","Rapport Indicateur Technicien", text);
        //     TempData["idprojet"] = oa.IdProjet;
        //     return RedirectToAction("RealIndex","OccurenceActivite");
        // }
        // else
        // {
        //     TempData["messageerreur"] = messageerreur;
        //     TempData["idoccurence"] = idoccurence;
        //     return RedirectToAction("VersInsertionIndicateurAvecDate");
        // }
        return RedirectToAction("VersInsertionIndicateurAvecDate");
    }

    public IActionResult versAffectationTechnicien(int? idprojet)
    {
        if (idprojet == null)
        {
            idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
        }
        else
        {
            HttpContext.Session.SetString("idprojet", idprojet+"");
            idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
        }
        List<Technicien> listetechnicien = _context.Technicien
            .Where(a => a.isAffected == false).ToList();
        ViewData["listetechnicien"] = listetechnicien;
        ViewBag.idprojet = idprojet;
        return View("~/Views/AffectationTechnicien/PageAffectation.cshtml");
    }
    public void affectationTechnicien(List<int> idtechnicien, int idprojet)
    {
        string messageerreur = "";
        foreach (var v in idtechnicien)
        {
            Technicien technicien = _context.Technicien
                .First(a => a.Id == v);
            if (!technicien.isAffected)
            {
                TechnicienProjet technicienProjet = new TechnicienProjet()
                {
                    IdProjet = idprojet,
                    IdTechnicien = v
                };
                _context.Add(technicienProjet);
                technicien.isAffected = true;
                _context.SaveChanges();
            }
            else
            {
                messageerreur += "technicien "+ technicien.Email +" deja pris dans un autre projet";
            }
            ViewBag.messageerreur = messageerreur;
        }
    }

    public IActionResult VoirListeTechnicien(int? idprojet)
    {
        if (idprojet == null)
        {
            idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
        }
        else
        {
            HttpContext.Session.SetString("idprojet", idprojet+"");
            idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
        }

        List<OccurenceActivite> listeoccurence = _context.ActiviteProjet
            .Include(z => z.Activite)
            .Where(a => a.IdProjet == idprojet).ToList();
        List<TechnicienProjet> listetechnicienprojet = _context.TechnicienProjet
            .Include(p => p.Technicien)
            .Where(a => a.IdProjet == idprojet).ToList();
        ViewData["listetechnicienprojet"] = listetechnicienprojet;
        ViewData["listeoccurence"] = listeoccurence;
        return View("~/Views/AffectationTechnicien/ListeTechnicienParProjet.cshtml");
    }
    
    public IActionResult VersPageTravailTechnicien(int idtechnicien, int idoccurence)
    {
        // int idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
        List<Site> listesite = _context.Site
            .Include(a => a.Commune)
            .Include(a => a.District)
            .Include(a => a.Region)
            .ToList();
        List<ViewSite> vs = _context.ViewSite
            .Include(z => z.TypeIndicateur)
            .Where(a => a.IdOccurence == idoccurence).ToList();
        ViewData["listeindicateur"] = vs;
        ViewData["listesite"] = listesite;
        ViewBag.idtechnicien = idtechnicien;
        ViewBag.idoccurence = idoccurence;
        return View("~/Views/AffectationTechnicien/PageTravailTechnicien.cshtml");
    }
    
    public IActionResult NewSite(int idoccurence, int idtechnicien,string libelle, int region, int district, int commune)
    {
        // int idprojet = Int32.Parse(HttpContext.Session.GetString("idprojet"));
        List<Site> listesite = _context.Site
            .Include(a => a.Commune)
            .Include(a => a.District)
            .Include(a => a.Region)
            .ToList();
        Site site = new Site()
        {
            Libelle = libelle,
            IdCommune = commune,
            IdDistrict = district,
            IdRegion = region
        };
        _context.Add(site);
        _context.SaveChanges();
        Site newsite = _context.Site.First(a => a == site);
        ViewBag.idsite = newsite.Id;
        List<ViewSite> vs = _context.ViewSite
            .Include(z => z.TypeIndicateur)
            .Where(a => a.IdOccurence == idoccurence).ToList();
        ViewData["listeindicateur"] = vs;
        ViewData["idtechnicien"] = idtechnicien; 
        ViewData["listesite"] = listesite;
        ViewBag.idoccurence = idoccurence;
        return View("~/Views/AffectationTechnicien/PageTravailTechnicien.cshtml");
    }
    
    public IActionResult versAjoutRapportSource(int idoccurence)
    {
        List<OccurenceSourceDeVerification> liste = _context
            .ActiviteProjetSourceDeVerification
            .Include(p => p.SourceDeVerification)
            .Where(a => a.IdOccurence == idoccurence).ToList();
        ViewData["listesource"] = liste;
        return View("~/Views/PagesSourceDeVerification/AcceuilAjout.cshtml");
    }

    public void InsertionTechnicienSite(int idtypeindicateur, int idsite, int idtechnicien, string target)
    {
        TechnicienSite technicienSite = new TechnicienSite()
        {
            IdSite = idsite,
            IdTechnicien = idtechnicien,
            IdIndicateur = idtypeindicateur,
            Target = Double.Parse(target)
        };
        _context.Add(technicienSite);
        _context.SaveChanges();
    }

    public void InsertionRapportTechnicienSite(int idsite, int idindicateur, string targeteffectue, DateOnly dateaction)
    {
        int idtechnicien = Int32.Parse(HttpContext.Session.GetString("idtechnicien"));
        double effectuedtarget = Double.Parse(targeteffectue);
        RapportTechnicienSite rapportTechnicienSite = new RapportTechnicienSite()
        {
            IdSite = idsite,
            IdTechnicien = idtechnicien,
            IdIndicateur = idindicateur,
            TargetEffectue = effectuedtarget,
            DateAction = dateaction
        };
        _context.Add(rapportTechnicienSite);
        _context.SaveChanges();
    }

    public IActionResult versInsertionSite(int idtechnicien, int idoccurence)
    {
        List<Region> listeregion = _context.Region.ToList();
        List<District> listedistrict = _context.District.ToList();
        List<Commune> listecommune = _context.Commune.ToList();
        ViewData["listeregion"] = listeregion;
        ViewData["listedistrict"] = listedistrict;
        ViewData["listecommune"] = listecommune;
        ViewBag.idtechnicien = idtechnicien;
        ViewBag.idoccurence = idoccurence;
        return View("~/Views/Site/InsertionSite.cshtml");
    }

    public IActionResult versInsertionRapportIndicateurTechnicien(int idsite)
    {
        List<TechnicienSite> liste = _context.TechnicienSite
            .Include(z => z.TypeIndicateur)
            .Include(y => y.Site)
            .Where(a => a.IdSite == idsite).ToList();
        ViewData["listetechniciensite"] = liste;
        ViewBag.idsite = idsite;
        return View("~/Views/FrontTechnicien/PageIndicateurTechnicien.cshtml");
    }

    public IActionResult versPageInsertionRapport(int idsite, int idtypeindicateur)
    {
        TechnicienSite tc = _context.TechnicienSite
            .Include(a => a.TypeIndicateur)
            .Include(a => a.Site)
            .First(a => a.IdSite == idsite && a.IdIndicateur == idtypeindicateur);
        ViewData["techniciensite"] = tc;
        ViewBag.idsite = idsite;
        ViewBag.idtypeindicateur = idtypeindicateur;
        return View("~/Views/FrontTechnicien/PageInsertionRapportSurSite.cshtml");
    }
}