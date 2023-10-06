using System.Reflection.Metadata;
using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class ExportPDFController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExportPDFController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersExportProjet()
    {
        return View("~/Views/ExportPDF/Choice.cshtml");
    }
    
    public string nomResultatById(int id)
    {
        return _context.OccurenceResultat
            .Include(a => a.Resultat)   
            .First(a => a.IdResultat == id)
            .Resultat
            .Nom;
    }

    public string nomActiviteById(int id)
    {
        return _context.OccurenceActivite
            .Include(a => a.Activite)
            .First(a => a.Id == id)
            .Activite
            .Nom;
    }

    public string nomSousActiviteById(int id)
    {
        return _context.OccurenceSousActivite
            .Include(a => a.SousActivite)
            .First(a => a.Id == id)
            .SousActivite
            .Nom;
    }

    public IActionResult VersMainPage(int resultat, int activite, int sousactivite, int budgetresultat, int budgetactivite, int budgetsousactivite)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        List<OccurenceResultat> resultats = new List<OccurenceResultat>();
        List<OccurenceActivite> listeoa = new List<OccurenceActivite>();
        List<OccurenceSousActivite> listeosa = new List<OccurenceSousActivite>();
        
        if (resultat == 1)
        {
            resultats = _context.OccurenceResultat
                .Where(a => a.IdProjet == idprojet)
                .ToList();
        }

        if (activite == 1)
        {
            foreach (var v in resultats)
            {
                List<OccurenceActivite> listeoccurenceactivite = _context.OccurenceActivite
                    .Where(a => a.IdOccurenceResultat == v.Id)
                    .OrderBy(a => a.IdOccurenceResultat)
                    .ToList();
                listeoa.AddRange(listeoccurenceactivite);
                if (sousactivite == 1)
                {
                    foreach (var z in listeoccurenceactivite)
                    {
                        List<OccurenceSousActivite> listeoccurencesousactivite = _context
                            .OccurenceSousActivite
                            .Where(a => a.IdOccurenceActivite == z.Id)
                            .OrderBy(a => a.IdOccurenceActivite)
                            .ToList();
                        listeosa.AddRange(listeosa);
                    }
                }
            }
        }

        ViewData["listeoccurenceresultat"] = resultats;
        ViewData["listeoccurenceactivite"] = listeoa;
        ViewData["listeoccurencesousactivite"] = listeosa;
        return View("MainPage");
    }
}