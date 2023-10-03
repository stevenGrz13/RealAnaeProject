using System.Reflection.Metadata;
using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;

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

    public IActionResult VersMainPage(int resultat, int budgetresultat, int activite, int budgetactivite, int sousactivite, int budgetsousactivite)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        List<OccurenceResultat> listeresultat = new List<OccurenceResultat>();
        List<OccurenceActivite> listeactivite = new List<OccurenceActivite>();
        List<OccurenceSousActivite> listesousactivite = new List<OccurenceSousActivite>();
        var resultatfinal = 0;
        if (resultat == 1)
        {
            listeresultat = _context.OccurenceResultat
                .Where(a => a.IdProjet == idprojet)
                .ToList();
        }

        if (activite == 1)
        {
            foreach (var v in listeresultat)
            {
                listeactivite.AddRange(_context.OccurenceActivite.Where(a => a.IdOccurenceResultat == v.Id).ToList());
            }
        }

        if (sousactivite == 1)
        {
            foreach (var v in listeactivite)
            {
                listesousactivite.AddRange(_context.OccurenceSousActivite.Where(a => a.IdOccurenceActivite == v.Id)
                    .ToList());
            }
        }
        return View("MainPage");
    }
}