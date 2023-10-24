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

    public IActionResult VersExportProjet(int idprojet)
    {
        ViewBag.idprojet = idprojet;
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
                .Where(a => a.IdProjet == idprojet && a.IsSupp == false)
                .ToList();
        }

        if (activite == 1)
        {
            foreach (var v in resultats)
            {
                List<OccurenceActivite> listeoccurenceactivite = _context.OccurenceActivite
                    .Where(a => a.IdOccurenceResultat == v.Id && a.IsSupp == false)
                    .OrderBy(a => a.IdOccurenceResultat)
                    .ToList();
                listeoa.AddRange(listeoccurenceactivite);
                if (sousactivite == 1)
                {
                    foreach (var z in listeoccurenceactivite)
                    {
                        List<OccurenceSousActivite> listeoccurencesousactivite = _context
                            .OccurenceSousActivite
                            .Where(a => a.IdOccurenceActivite == z.Id && a.IsSupp == false)
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
    
    public static string RenderViewToString(ControllerBase controller, string viewName, object model)
    {
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    
        var services = new ServiceCollection()
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddScoped<ITempDataProvider, SessionStateTempDataProvider>()
            .BuildServiceProvider();
    
        httpContext.RequestServices = services;
    
        var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
        var viewResult = viewEngine.FindView(actionContext, viewName, false);
    
        if (viewResult.Success)
        {
            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };
    
            using (var writer = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(httpContext, services.GetRequiredService<ITempDataProvider>()),
                    writer,
                    new HtmlHelperOptions()
                );
    
                viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();
                return writer.ToString();
            }
        }
    
        return null;
    }
}