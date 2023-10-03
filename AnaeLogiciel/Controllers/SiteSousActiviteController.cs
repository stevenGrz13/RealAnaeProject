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
        ViewData["listecommune"] = _context.Commune.ToList();
        ViewData["listedistrict"] = _context.District.ToList();
        ViewData["listeregion"] = _context.Region.ToList();
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
        ViewData["indicateurtechniciensitesousactivite"] = _context.IndicateurTechnicienSiteSousActivite
            .Include(a => a.TypeIndicateur)
            .Include(a => a.Technicien)
            .Where(a => a.IdSiteSousActivite == idsitesousactivite)
            .ToList();
        SiteActivite st = _context.SiteActivite
            .First(a => a.Id == idsitesousactivite);
        List<OccurenceActiviteIndicateur> liste = _context
            .OccurenceActiviteIndicateur
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdOccurenceActivite == st.IdOccurenceActivite)
            .ToList();
        ViewData["listeindicateur"] = liste;
        int idpprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        ViewBag.idsitesousactivite = idsitesousactivite;
        ViewData["listetechnicien"] = _context
            .TechnicienProjet
            .Include(a => a.Technicien)
            .Where(a => a.IdProjet == idpprojet).ToList();
        ViewBag.idsitesousactivite = idsitesousactivite;
        return View("~/Views/SiteSousActivite/Details.cshtml");
    }

    public void CreateWithIndicateur(int idsitesousactivite, int idindicateur, int idtechnicien, string target)
    {
        ViewData["indicateurtechniciensitesousactivite"] = _context.IndicateurTechnicienSiteSousActivite
            .Include(a => a.TypeIndicateur)
            .Include(a => a.Technicien)
            .Where(a => a.IdSiteSousActivite == idsitesousactivite)
            .ToList();
        IndicateurTechnicienSiteSousActivite ic = new IndicateurTechnicienSiteSousActivite()
        {
            IdSiteSousActivite = idsitesousactivite,
            IdIndicateur = idindicateur,
            IdTechnicien = idtechnicien,
            Target = Double.Parse(target)
        };
        _context.Add(ic);
        _context.SaveChangesAsync();
        // SiteSousActivite st = _context.SiteSousActivite
        //     .First(a => a.Id == idsitesousactivite);
        // List<OccurenceSousActiviteIndicateur> liste = _context
        //     .OccurenceSousActiviteIndicateur
        //     .Include(a => a.TypeIndicateur)
        //     .Where(a => a.IdOccurenceSousActivite == st.IdOccurenceSousActivite)
        //     .ToList();
        // ViewData["listeindicateur"] = liste;
        // int idpprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        // ViewBag.idsitesousactivite = idsitesousactivite;
        // ViewData["listetechnicien"] = _context
        //     .TechnicienProjet
        //     .Include(a => a.Technicien)
        //     .Where(a => a.IdProjet == idpprojet).ToList();
        // ViewBag.idsiteactivite = idsitesousactivite;
        // return View("~/Views/SiteSousActivite/Details.cshtml");
    }
}