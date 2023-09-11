using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnaeLogiciel.Controllers;

public class InsertionSourcesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public InsertionSourcesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult versInsertionGlobalSourcedeVerification(int idoccurence, int idtechnicien, int idsourcedeverification, DateOnly dateaction)
    {
        ViewBag.idoccurence = idoccurence;
        ViewBag.idtechnicien = idtechnicien;
        ViewBag.dateaction = dateaction;
        ViewBag.idsourcedeverification = idsourcedeverification;
        SourceDeVerification src = _context.SourceDeVerification
                .First(a => a.Id == idsourcedeverification);
        if (src.Id == 1) // pv/rapport
        {
            return View("~/Views/PagesSourceDeVerification/insertionRapport.cshtml");
        }
        if (src.Id == 2) // fiche de presence
        {
            return View("~/Views/PagesSourceDeVerification/insertionFichedepresence.cshtml");
        }
        if (src.Id == 3) // photo
        {
            return View("~/Views/PagesSourceDeVerification/insertionPhoto.cshtml");
        }
        if (src.Id == 4) // etat de paiement
        {
            return View("~/Views/PagesSourceDeVerification/insertionEtatdepaiement.cshtml");
        }
        else // bon de reception
        {
            return View("~/Views/PagesSourceDeVerification/insertionBondereception.cshtml");
        }
    }

    public void insertionPhoto(int idsourcedeverification, int idoccurence, int idtechnicien, IFormFile imageFile, DateOnly dateaction)
    {
        string date = (dateaction + "").Replace("/", ".");
        Technicien t = _context.Technicien.First(a => a.Id == 1);
        OccurenceSourceDeVerificationAvecDate occ = new OccurenceSourceDeVerificationAvecDate()
        {
            IdOccurence = idoccurence,
            IdTechnicien = 1,
            IdSourcedeverification = idsourcedeverification,
            DateAction = dateaction,
            Fichier = Fonction.Fonction.ImportPhoto(imageFile, "occurence"+idoccurence+"/"+"photo","Date."+date+".Technicien."+t.Email, _webHostEnvironment)
        };
        _context.Add(occ);
        _context.SaveChanges();
    }
    
    public void insertionRapportPV(int idsourcedeverification, int idoccurence, int idtechnicien, IFormFile fileUpload, DateOnly dateaction)
    {
        string date = (dateaction + "").Replace("/", ".");
        int realidtechnicien = Int32.Parse(HttpContext.Session.GetString("idtechnicien"));
        Technicien t = _context.Technicien.First(a => a.Id == 1);
        OccurenceSourceDeVerificationAvecDate occ = new OccurenceSourceDeVerificationAvecDate()
        {
            IdOccurence = idoccurence,
            IdTechnicien = realidtechnicien,
            IdSourcedeverification = idsourcedeverification,
            DateAction = dateaction,
            Fichier = Fonction.Fonction.ImportFileTxt(fileUpload, "occurence"+idoccurence+"/"+"rapport-pv","Date."+ date +".Technicien."+t.Email+".txt", _webHostEnvironment)
        };
        _context.Add(occ);
        _context.SaveChanges();
    }
}