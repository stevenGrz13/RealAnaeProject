﻿using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class SiteActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public SiteActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IActionResult VersInsertionSiteActivite(int idoccurenceactivite)
    {
        ViewData["listecommune"] = _context.Commune.ToList();
        ViewData["listedistrict"] = _context.District.ToList();
        ViewData["listeregion"] = _context.Region.ToList();
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/SiteActivite/Insertion.cshtml");
    }

    public IActionResult Create(string libelle, int commune, int region, int district, int idoccurenceactivite)
    {
        SiteActivite st = new SiteActivite()
        {
            IdOccurenceActivite = idoccurenceactivite,
            Libelle = libelle,
            IdCommune = commune,
            IdDistrict = district,
            IdRegion = region
        };
        _context.Add(st);
        _context.SaveChanges();
        ViewData["occurenceactivite"] = _context.OccurenceActivite
            .Include(a => a.Activite)
            .FirstOrDefault(a => a.Id == idoccurenceactivite);
        ViewData["listeoccurenceactiviteindicateur"] =
            _context.OccurenceActiviteIndicateur
                .Include(a => a.TypeIndicateur)
                .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        ViewData["listesiteoccurenceactivite"] = _context.SiteActivite
            .Include(a => a.Commune)
            .Include(a => a.District)
            .Include(a => a.Region)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        ViewBag.idoccurencesousactivite = idoccurenceactivite;
        return View("~/Views/OccurenceActivite/Details.cshtml");
    }

    public IActionResult VersDetailsSiteActivite(int idsiteactivite)
    {
        ViewData["indicateurtechniciensiteactivite"] = _context.IndicateurTechnicienSiteActivite
            .Include(a => a.TypeIndicateur)
            .Include(a => a.Technicien)
            .Where(a => a.IdSiteActivite == idsiteactivite)
            .ToList();
        SiteActivite st = _context.SiteActivite
            .First(a => a.Id == idsiteactivite);
        List<OccurenceActiviteIndicateur> liste = _context
            .OccurenceActiviteIndicateur
            .Include(a => a.TypeIndicateur)
            .Where(a => a.IdOccurenceActivite == st.IdOccurenceActivite)
            .ToList();
        ViewData["listeindicateur"] = liste;
        int idpprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        ViewBag.idsiteactivite = idsiteactivite;
        ViewData["listetechnicien"] = _context
            .TechnicienProjet
            .Include(a => a.Technicien)
            .Where(a => a.IdProjet == idpprojet).ToList();
        ViewBag.idsiteactivite = idsiteactivite;
        return View("~/Views/SiteActivite/Details.cshtml");
    }

    public void CreateWithIndicateur(int idsiteactivite, int idindicateur, int idtechnicien, string target)
    {
        ViewData["indicateurtechniciensiteactivite"] = _context.IndicateurTechnicienSiteActivite
            .Include(a => a.TypeIndicateur)
            .Include(a => a.Technicien)
            .Where(a => a.IdSiteActivite == idsiteactivite)
            .ToList();
        IndicateurTechnicienSiteActivite ic = new IndicateurTechnicienSiteActivite()
        {
            IdSiteActivite = idsiteactivite,
            IdIndicateur = idindicateur,
            IdTechnicien = idtechnicien,
            Target = Double.Parse(target)
        };
        _context.Add(ic);
        _context.SaveChanges();
        // SiteActivite st = _context.SiteActivite
        //     .First(a => a.Id == idsiteactivite);
        // List<OccurenceActiviteIndicateur> liste = _context
        //     .OccurenceActiviteIndicateur
        //     .Include(a => a.TypeIndicateur)
        //     .Where(a => a.IdOccurenceActivite == st.IdOccurenceActivite)
        //     .ToList();
        // ViewData["listeindicateur"] = liste;
        // int idpprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        // ViewBag.idsiteactivite = idsiteactivite;
        // ViewData["listetechnicien"] = _context
        //     .TechnicienProjet
        //     .Include(a => a.Technicien)
        //     .Where(a => a.IdProjet == idpprojet).ToList();
        // ViewBag.idsiteactivite = idsiteactivite;
        // return View("~/Views/SiteActivite/Details.cshtml");
    }
}