using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class SousActiviteSourceTechnicienController : Controller
{
    private readonly ApplicationDbContext _context;

    public SousActiviteSourceTechnicienController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersListeRapport(int idoccurencesousactivite)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        List<TechnicienProjet> tech = _context
            .TechnicienProjet
            .Include(a => a.Technicien)
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        ViewData["listetech"] = tech;
        return View("~/Views/SousActiviteSourceTechnicien/Liste.cshtml");
    }

    public IActionResult ListeRapportTech(int idtechnicien, int idoccurencesousactivite)
    {
        Technicien t = _context.Technicien
            .FirstOrDefault(a => a.Id == idtechnicien);
        List<OccurenceSousActiviteSourceTechnicien> liste = _context
            .OccurenceSousActiviteSourceTechnicien
            .Include(a => a.SourceDeVerification)
            .Where(a => a.IdTechnicien == idtechnicien && a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        ViewData["listesourcetech"] = liste;
        if (t != null)
        {
            ViewBag.nomtechnicien = t.Email;   
        }

        ViewBag.idtechnicien = idtechnicien;
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        return View("~/Views/SousActiviteSourceTechnicien/ListeTech.cshtml");
    }

    public IActionResult VersInsertion(int idtechnicien, int idoccurencesousactivite)
    {
        ViewData["listesource"] = _context
            .OccurenceSousActiviteSource
            .Include(a => a.SourceDeVerification)
            .Where(a => a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        ViewBag.idtechnicien = idtechnicien;
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        return View("~/Views/SousActiviteSourceTechnicien/Insertion.cshtml");
    }
    
    public IActionResult Create(int idoccurencesousactivite, int idtechnicien, int idsource, string lienfichier, DateOnly dateaction)
    {
        Technicien t = _context.Technicien
            .FirstOrDefault(a => a.Id == idtechnicien);
        OccurenceSousActiviteSourceTechnicien oa = new OccurenceSousActiviteSourceTechnicien()
        {
            IdOccurenceSousActivite = idoccurencesousactivite,
            IdSource = idsource,
            IdTechnicien = idtechnicien,
            LienFichier = lienfichier,
            DateAction = dateaction
        };
        _context.Add(oa);
        _context.SaveChanges();
        List<OccurenceSousActiviteSourceTechnicien> liste = _context
            .OccurenceSousActiviteSourceTechnicien
            .Include(a => a.SourceDeVerification)
            .Where(a => a.IdTechnicien == idtechnicien && a.IdOccurenceSousActivite == idoccurencesousactivite)
            .ToList();
        ViewData["listesourcetech"] = liste;
        if (t != null)
        {
            ViewBag.nomtechnicien = t.Email;   
        }
        ViewBag.idtechnicien = idtechnicien;
        ViewBag.idoccurencesousactivite = idoccurencesousactivite;
        return View("~/Views/SousActiviteSourceTechnicien/ListeTech.cshtml");
    }
}