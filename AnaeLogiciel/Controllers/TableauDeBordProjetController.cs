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
        double somme = 0;
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        List<OccurenceResultat> listeres = _context.OccurenceResultat
            .Where(a => a.IdProjet == idprojet)
            .ToList();
        List<Vsommepaiementactivite> listeac = new List<Vsommepaiementactivite>();
        List<Vsommepaiementsousactivite> listesa = new List<Vsommepaiementsousactivite>();

        foreach (var v in listeres)
        {
            List<Vsommepaiementactivite> r = _context.Vsommepaiementactivite
                .Include(a => a.OccurenceActivite)
                .Where(a => a.IdOccurenceResultat == v.Id)
                .ToList();
            listeac.AddRange(r);
        }

        foreach (var v in listeac)
        {
            List<Vsommepaiementsousactivite> r = _context
                .Vsommepaiementsousactivite
                .Include(a => a.OccurenceSousActivite)
                .Where(a => a.IdOccurenceActivite == v.IdOccurenceActivite)
                .ToList();
            Console.WriteLine("TAILLE NIGGA="+r.Count);
            listesa.AddRange(r);
        }

        foreach (var v in listeac)
        {
            somme += v.Somme;
        } 
        
        foreach (var v in listesa)
        {
            somme += v.Somme;
        } 
        
        ViewData["listeactivite"] = listeac;
        Console.WriteLine("taille any activite=");
        Console.WriteLine(listeac.Count);
        Console.WriteLine("taille any sous acticite=");
        Console.WriteLine(listesa.Count);
        ViewData["listesousactivite"] = listesa;

        Projet projet = _context.Projet
            .First(a => a.Id == idprojet);
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
            foreach (var v in listeac)
            {
                v.Somme = v.Somme / projet.ValeurDevise;
            }

            foreach (var v in listesa)
            {
                v.Somme = v.Somme / projet.ValeurDevise;
            }
            ViewBag.budget = projet.Budget / projet.ValeurDevise;
            ViewBag.depense = somme / projet.ValeurDevise;
            ViewBag.reste = (projet.Budget - somme) / projet.ValeurDevise;   
        }

        List<Devise> listedevise = _context.Devise
            .Where(a => a.Id == projet.IdDevise)
            .ToList();
        Devise ariary = _context.Devise.First(a => a.Id == 1);
        listedevise.Add(ariary);
        ViewData["listedevise"] = listedevise;
        ViewData["projet"] = _context.Projet
            .First(a => a.Id == idprojet);
        return View("~/Views/Projet/TableauDeBord.cshtml");
    }
}