using System.Net.Mime;
using System.Reflection.Metadata;
using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using GrapeCity.Documents.Html;
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

    public IActionResult VersMainPage(int resultat, int activite, int sousactivite, int budgetresultat, int budgetactivite, int budgetsousactivite)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        List<OccurenceResultat> resultats = new List<OccurenceResultat>();
        List<OccurenceActivite> listeoa = new List<OccurenceActivite>();
        List<OccurenceSousActivite> listeosa = new List<OccurenceSousActivite>();

        Projet projet = _context.Projet.First(a => a.Id == idprojet);
        
        if (resultat == 1)
        {
            resultats = _context.OccurenceResultat
                .Include(a => a.Resultat)
                .Where(a => a.IdProjet == idprojet)
                .ToList();
        }

        if (activite == 1)
        {
            foreach (var v in resultats)
            {
                List<OccurenceActivite> listeoccurenceactivite = _context.OccurenceActivite
                    .Include(a => a.Activite)
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
                            .Include(a => a.SousActivite)
                            .Where(a => a.IdOccurenceActivite == z.Id)
                            .OrderBy(a => a.IdOccurenceActivite)
                            .ToList();
                        listeosa.AddRange(listeoccurencesousactivite);
                    }
                }
            }
        }

        ViewData["projet"] = projet;
        ViewData["listeoccurenceresultat"] = resultats;
        ViewData["listeoccurenceactivite"] = listeoa;
        ViewData["listeoccurencesousactivite"] = listeosa;
        return View("MainPage");
    }
    
    public async Task<IActionResult> GeneratePdf()
    {
        var tmp = Path.GetTempFileName();

        var req = HttpContext.Request;

        var uri = new Uri($"{req.Scheme}://{req.Host}{req.PathBase}/ExportPDF/");
        
        // var uri = new Uri($"{req.Scheme}://{req.Host}{req.PathBase}/ExportPDF/"+value);
        
        var browserPath = BrowserFetcher.GetSystemChromePath();

        using var browser = new GcHtmlBrowser(browserPath);

        using var htmlPage = browser.NewPage(uri);

        PdfOptions pdfOptions = new PdfOptions()
        {
            PageRanges = "1-100",
            Margins = new PdfMargins(0.2f),
            Landscape = false,
            PreferCSSPageSize = true
        };

        htmlPage.SaveAsPdf(tmp, pdfOptions);
        var stream = new MemoryStream();
        using (var ts = System.IO.File.OpenRead(tmp))
            ts.CopyTo(stream);
        System.IO.File.Delete(tmp);
        return File(stream.ToArray(), MediaTypeNames.Application.Pdf, "document.pdf");
    }

}