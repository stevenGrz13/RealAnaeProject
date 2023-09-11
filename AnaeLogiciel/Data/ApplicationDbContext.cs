using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AnaeLogiciel.Models;

namespace AnaeLogiciel.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<AnaeLogiciel.Models.Admin> Admin { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.Employe> Employe { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.Technicien> Technicien { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.Projet> Projet { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.Activite> Activite { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.SourceDeVerification> SourceDeVerification { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.TypeIndicateur> TypeIndicateur { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.OccurenceActivite> ActiviteProjet { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.DateRealisationProjet> DateRealisationProjet { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.AssociationIndicateurActivite> IndicateurActiviteProjet { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceSourceDeVerification> ActiviteProjetSourceDeVerification { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceSourceDeVerificationAvecDate> ActiviteProjetSourceDeVerificationAvecDate { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.TechnicienProjet> TechnicienProjet { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.Province> Province { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.Region> Region { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.District> District { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.Commune> Commune { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.Site> Site { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.TechnicienSite> TechnicienSite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.RapportTechnicienSite> RapportTechnicienSite { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.ViewSite> ViewSite { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.TechProj> TechProj { get; set; } = default!;
}