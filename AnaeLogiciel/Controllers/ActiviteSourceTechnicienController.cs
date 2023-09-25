using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class ActiviteSourceTechnicienController : Controller
{
    private readonly ApplicationDbContext _context;

    public ActiviteSourceTechnicienController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersListeRapport(int idoccurenceactivite)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        List<TechnicienProjet> tech = _context
            .TechnicienProjet
            .Include(a => a.Technicien)
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        ViewData["listetech"] = tech;
        return View("~/Views/ActiviteSourceTechnicien/Liste.cshtml");
    }

    public IActionResult ListeRapportTech(int idtechnicien, int idoccurenceactivite)
    {
        Technicien t = _context.Technicien
            .FirstOrDefault(a => a.Id == idtechnicien);
        List<OccurenceActiviteSourceTechnicien> liste = _context
            .OccurenceActiviteSourceTechnicien
            .Include(a => a.SourceDeVerification)
            .Where(a => a.IdTechnicien == idtechnicien && a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewData["listesourcetech"] = liste;
        if (t != null)
        {
            ViewBag.nomtechnicien = t.Email;   
        }

        ViewBag.idtechnicien = idtechnicien;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/ActiviteSourceTechnicien/ListeTech.cshtml");
    }

    public IActionResult VersInsertion(int idtechnicien, int idoccurenceactivite)
    {
        ViewData["listesource"] = _context
            .OccurenceActiviteSource
            .Include(a => a.SourceDeVerification)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewBag.idtechnicien = idtechnicien;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/ActiviteSourceTechnicien/Insertion.cshtml");
    }
    
    public IActionResult Create(int idoccurenceactivite, int idtechnicien, int idsource, string lienfichier, DateOnly dateaction)
    {
        Console.WriteLine("idtechnicien="+idtechnicien);
        Console.WriteLine("idoccurenceactivite="+idoccurenceactivite);
        Technicien t = _context.Technicien
            .FirstOrDefault(a => a.Id == idtechnicien);
        OccurenceActiviteSourceTechnicien oa = new OccurenceActiviteSourceTechnicien()
        {
            IdOccurenceActivite = idoccurenceactivite,
            IdSource = idsource,
            IdTechnicien = idtechnicien,
            LienFichier = lienfichier,
            DateAction = dateaction
        };
        _context.Add(oa);
        _context.SaveChanges();
        List<OccurenceActiviteSourceTechnicien> liste = _context
            .OccurenceActiviteSourceTechnicien
            .Include(a => a.SourceDeVerification)
            .Where(a => a.IdTechnicien == idtechnicien && a.IdOccurenceActivite == idoccurenceactivite)
            .ToList();
        ViewData["listesourcetech"] = liste;
        if (t != null)
        {
            ViewBag.nomtechnicien = t.Email;   
        }
        ViewBag.idtechnicien = idtechnicien;
        ViewBag.idoccurenceactivite = idoccurenceactivite;
        return View("~/Views/ActiviteSourceTechnicien/ListeTech.cshtml");
    }
}