using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class TableauDeBordProjetController : Controller
{
    private readonly ApplicationDbContext _context;

    public TableauDeBordProjetController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult VersTableau(int? iddevise)
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        Projet projet = _context.Projet
            .First(a => a.Id == idprojet);
        List<OccurenceResultat> listeresultat = _context.OccurenceResultat
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        List<OccurenceActivite> listeactivite = new List<OccurenceActivite>();

        foreach (var v in listeresultat)
        {
            listeactivite.AddRange(_context.OccurenceActivite
                .Where(a => a.IdOccurenceResultat == v.Id)
                .ToList());
        }

        List<Vsommepaiementsousactivite> listepaiement = new List<Vsommepaiementsousactivite>();

        foreach (var v in listeactivite)
        {
            listepaiement.AddRange(_context.Vsommepaiementsousactivite
                .Include(a => a.OccurenceActivite)
                .Where(a => a.IdOccurenceActivite == v.Id)
                .ToList());
        }
        
        List<Devise> listedevise = _context.Devise
            .Where(a => a.Id == projet.IdDevise)
            .ToList();
        
        Devise ariary = _context.Devise.First(a => a.Id == 1);
        listedevise.Add(ariary);

        double somme = 0;

        foreach (var v in listepaiement)
        {
            somme += v.Somme;
        }

        Console.WriteLine("TAILLE="+listepaiement.Count);
        
        ViewData["listedevise"] = listedevise;
        ViewData["listepaiement"] = listepaiement;
        
        if (iddevise == null)
        {
            iddevise = 1;
        }
        if (iddevise == 1)
        {
            ViewBag.budget = projet.Budget;
            ViewBag.depense = somme;
            ViewBag.reste = projet.Budget - somme;
        }
        else
        {
            foreach (var v in listepaiement)
            {
                v.Somme = v.Somme / projet.ValeurDevise;
            }
            ViewBag.budget = projet.Budget / projet.ValeurDevise;
            ViewBag.depense = somme / projet.ValeurDevise;
            ViewBag.reste = (projet.Budget - somme) / projet.ValeurDevise; 
        }
        return View("~/Views/Projet/TableauDeBord.cshtml");
    }
}