using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class SiteSousActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public SiteSousActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IActionResult VersInsertionSiteSousActivite(int idoccurencesousactivite)
    {
        OccurenceSousActivite os = _context
            .OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite);
        ViewData["listecommune"] = _context.Commune.ToList();
        ViewData["listedistrict"] = _context.District.ToList();
        ViewData["listeregion"] = _context.Region.ToList();
        ViewBag.idoccurenceactivite = os.IdOccurenceActivite;
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        return View("~/Views/SiteSousActivite/Insertion.cshtml");
    }

    public IActionResult Create(string libelle, int commune, int region, int district, int idoccurencesousactivite)
    {
        OccurenceSousActivite oc = _context.OccurenceSousActivite
            .First(a => a.Id == idoccurencesousactivite);
        SiteSousActivite st = new SiteSousActivite()
        {
            IdOccurenceSousActivite = idoccurencesousactivite,
            Libelle = libelle,
            IdCommune = commune,
            IdDistrict = district,
            IdRegion = region
        };
        _context.Add(st);
        _context.SaveChanges();
        return RedirectToAction("VersDetailsOccurenceSousActivite","OccurenceSousActivite",new {idoccurencesousactivite = idoccurencesousactivite, idoccurenceactivite = oc.IdOccurenceActivite});
    }
    
    public IActionResult VersDetailsSiteSousActivite(int idsitesousactivite)
    {
        if (TempData["messageerreur"] != null)
        {
            ViewBag.messageerreur = TempData["messageerreur"];
        }
        ViewData["indicateurtechniciensitesousactivite"] = _context.IndicateurTechnicienSiteSousActivite
            .Include(a => a.TypeIndicateur)
            .Include(a => a.Technicien)
            .Where(a => a.IdSiteSousActivite == idsitesousactivite)
            .ToList();
        SiteSousActivite st = _context.SiteSousActivite
            .First(a => a.Id == idsitesousactivite);
        List<OccurenceSousActiviteIndicateur> liste = _context
            .OccurenceSousActiviteIndicateur
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdOccurenceSousActivite == st.IdOccurenceSousActivite)
            .ToList();
        ViewData["listeindicateur"] = liste;
        int idpprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        ViewBag.idsitesousactivite = idsitesousactivite;
        ViewData["listetechnicien"] = _context
            .TechnicienProjet
            .Include(a => a.Technicien)
            .Where(a => a.IdProjet == idpprojet).ToList();
        ViewBag.idsitesousactivite = idsitesousactivite;
        OccurenceSousActivite os = _context.OccurenceSousActivite
            .First(a => a.Id == st.IdOccurenceSousActivite);
        ViewBag.idoccurenceactivite = os.IdOccurenceActivite;
        ViewBag.idoccurencesousactivite = st.IdOccurenceSousActivite;
        return View("~/Views/SiteSousActivite/Details.cshtml");
    }

    public IActionResult CreateWithIndicateur(int idsitesousactivite, int idindicateur, int idtechnicien, string target)
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
            IndicateurTechnicienSiteSousActivite ic = new IndicateurTechnicienSiteSousActivite()
            {
                IdSiteSousActivite = idsitesousactivite,
                IdIndicateur = idindicateur,
                IdTechnicien = idtechnicien,
                Target = Double.Parse(target)
            };
            _context.Add(ic);
            _context.SaveChangesAsync();
            return RedirectToAction("VersDetailsSiteSousActivite","SiteSousActivite",new { idsitesousactivite = idsitesousactivite});   
        }
        else
        {
            TempData["messageerreur"] = messageerreur;
            return RedirectToAction("VersDetailsSiteSousActivite", new { idsitesousactivite = idsitesousactivite });
        }
    }
    
    public IActionResult VersMapSiteSousActivite(int idsitesousactivite)
    {
        SiteSousActivite site = _context.SiteSousActivite
            .Include(a => a.Commune)
            .First(a => a.Id == idsitesousactivite); 
        Region region = _context.Region
            .Include(a => a.Province)
            .First(a => a.Id == site.IdRegion);
        string province = region.Province.Nom;
        string commune = site.Commune.Nom;
        ViewBag.province = province;
        ViewBag.commune = commune;
        return View("~/Views/SiteSousActivite/Map.cshtml");
    }
}