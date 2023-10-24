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

    public IActionResult VersTableau()
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
        ViewData["listesousactivite"] = listesa;

        Projet projet = _context.Projet
            .First(a => a.Id == idprojet);
        ViewBag.budget = projet.Budget;
        ViewBag.depense = somme;
        ViewBag.reste = projet.Budget - somme;
        return View("~/Views/Projet/TableauDeBord.cshtml");
    }
}