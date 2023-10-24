using AnaeLogiciel.Data;
using AnaeLogiciel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnaeLogiciel.Controllers;

public class DateRealisationProjetController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly SmtpConfig _smtpConfig;
    
    public DateRealisationProjetController(ApplicationDbContext context, SmtpConfig smtpConfig)
    {
        _context = context;
        _smtpConfig = smtpConfig;
    }

    public async Task<IActionResult> FinalisationProjet(int idprojet)
    {
        Projet projet = _context.Projet
            .First(a => a.Id == idprojet);
        projet.FinishedOrNot = true;
        projet.Avancement = 101;
        Fonction.Fonction.EnvoyerEmail(_smtpConfig,"razafimahefasteven130102@gmail.com","travisjamesmdg7713@gmail.com","Avancement a 100%","Le projet "+projet.Nom+" est officiellement terminee");
        DateRealisationProjet dr = new DateRealisationProjet()
        {
            IdProjet = idprojet,
            DateDebutRealisation = projet.DateDebutPrevision,
            DateFinRealisation = DateOnly.FromDateTime(DateTime.Now)
        };
        _context.Add(dr);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Projet", new { idprojet = idprojet });
    }
    
    public IActionResult RefusFinalisationProjet(int idprojet)
    {
        return RedirectToAction("Details", "Projet", new { idprojet = idprojet });
    }
}