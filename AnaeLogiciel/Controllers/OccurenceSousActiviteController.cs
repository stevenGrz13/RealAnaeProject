using AnaeLogiciel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Controllers;

public class OccurenceSousActiviteController : Controller
{
    private readonly ApplicationDbContext _context;

    public OccurenceSousActiviteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult ListeOccurenceSousActivite(int idoccurenceactivite)
    {
        ViewData["listeoccurencesousactivite"] = _context.OccurenceSousActivite
            .Include(a => a.SousActivite)
            .Where(a => a.IdOccurenceActivite == idoccurenceactivite).ToList();
        return View("~/Views/OccurenceSousActivite/Liste.cshtml");
    }

    public IActionResult VersInsertionOccurenceSousActivite(int idoccurenceactivite)
    {
        return View("~/Views/OccurenceSousActivite/Liste.cshtml");
    }
}