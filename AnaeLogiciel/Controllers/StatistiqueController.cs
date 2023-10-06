using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AnaeLogiciel.Controllers;

public class StatistiqueController : Controller
{
    private readonly ApplicationDbContext _context;

    public StatistiqueController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult CalculStatistique()
    {
        int idprojet = HttpContext.Session.GetInt32("idprojet").GetValueOrDefault();
        Console.WriteLine("------------");
        Console.WriteLine("idprojet="+idprojet);
        Projet projet = _context.Projet.First(a => a.Id == idprojet);
        int idhomme = 6;
        int idfemme = 5;
        // 1 = homme
        // 2 = femme
        double[] res = new double[2];
        res[0] = 0;
        res[1] = 0;
        double sommehomme = 0;
        double sommefemme = 0;
        double pourcentagehomme = 0;
        double pourcentagefemme = 0;
        List<VCalculStatHommeFemme> listehomme = _context.VCalculStatHommeFemme
            .Where(a => a.IdProjet == idprojet && a.IdIndicateur == idhomme)
            .ToList();
        List<VCalculStatHommeFemme> listefemme = _context.VCalculStatHommeFemme
            .Where(a => a.IdProjet == idprojet && a.IdIndicateur == idfemme)
            .ToList();
        foreach (var v in listehomme)
        {
            sommehomme += v.Quantite;
        }
        foreach (var v in listefemme)
        {
            sommefemme += v.Quantite;
        }

        pourcentagehomme = (sommehomme * 100) / (sommehomme + sommefemme);
        pourcentagefemme = (sommefemme * 100) / (sommehomme + sommefemme);
        res[0] = pourcentagehomme;
        res[1] = pourcentagefemme;
        ViewData["projet"] = projet;
        PourcentageHommeFemme p = new PourcentageHommeFemme()
        {
            PourcentageHomme = (int) pourcentagehomme,
            PourcentageFemme = (int) pourcentagefemme
        };
        ViewData["pourcentage"] = p;
        return View("~/Views/Statistique/StatistiqueHommeFemme.cshtml");
    }
}