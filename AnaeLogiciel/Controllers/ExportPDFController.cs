using System.Net.Mime;
using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using GrapeCity.Documents.Html;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ContentDisposition = Rotativa.AspNetCore.Options.ContentDisposition;
using Document = System.Reflection.Metadata.Document;

namespace AnaeLogiciel.Controllers;

public class ExportPDFController : Controller
{
    private readonly ApplicationDbContext _context;

    private readonly ICompositeViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    
    public ExportPDFController(ApplicationDbContext context,ICompositeViewEngine viewEngine,
        ITempDataProvider tempDataProvider)
    {
        _context = context;
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
    }

    public IActionResult GeneretePDF()
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();

        Projet projet = _context.Projet
            .Include(a => a.Bailleur)
            .Include(a => a.Devise)
            .First(a => a.Id == idprojet);

        List<ProjetComposant> listecomposant = _context.ProjetComposant
            .Where(a => a.IdProjet == idprojet)
            .ToList();

        List<ProjetPartenaireTechnique> listepartenaire = _context
            .ProjetPartenaireTechnique
            .Where(a => a.IdProjet == idprojet)
            .ToList();

        List<Site> listesiteprojet = _context.Site
            .Include(a => a.Commune)
            .Include(a => a.Region)
            .Include(a => a.District)
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        
        if (projet.FinishedOrNot)
        {
            DateRealisationProjet daterealisaton = _context.DateRealisationProjet
                .First(a => a.IdProjet == idprojet);
            var modelPDF = new
            {
                listesite = listesiteprojet,
                nomprojet = projet.Nom,
                sigleprojet = projet.Sigle,
                deviseprojet = projet.Devise.Nom,
                valeurdevise = projet.ValeurDevise,
                composants = listecomposant,
                bailleur = projet.Bailleur.Nom, 
                partenaires = listepartenaire,
                datedebutprevision = projet.DateDebutPrevision,
                datefinprevision = projet.DateFinPrevision,
                datedebutrealisation = daterealisaton.DateDebutRealisation,
                datefinrealisation = daterealisaton.DateFinRealisation,
                finished = projet.FinishedOrNot
            };

            var pdfResult = new ViewAsPdf("ITOLEIZY", modelPDF);
            pdfResult.FileName = $"{projet.Sigle}.pdf";

            pdfResult.PageOrientation = Orientation.Landscape;
            pdfResult.PageSize = Rotativa.AspNetCore.Options.Size.A4;

            pdfResult.ContentDisposition = ContentDisposition.Attachment;
            return pdfResult;   
        }
        else
        {
            var modelPDF = new
            {
                listesite = listesiteprojet,
                nomprojet = projet.Nom,
                sigleprojet = projet.Sigle,
                deviseprojet = projet.Devise.Nom,
                valeurdevise = projet.ValeurDevise,
                composants = listecomposant,
                bailleur = projet.Bailleur.Nom, 
                partenaires = listepartenaire,
                datedebutprevision = projet.DateDebutPrevision,
                datefinprevision = projet.DateFinPrevision,
                finished = projet.FinishedOrNot
            };

            var pdfResult = new ViewAsPdf("ITOLEIZY", modelPDF);
            pdfResult.FileName = $"{projet.Sigle}.pdf";

            pdfResult.PageOrientation = Orientation.Landscape;
            pdfResult.PageSize = Rotativa.AspNetCore.Options.Size.A4;

            pdfResult.ContentDisposition = ContentDisposition.Attachment;
            return pdfResult;
        }
    }
}