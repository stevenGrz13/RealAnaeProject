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
    public DbSet<AnaeLogiciel.Models.DateRealisationProjet> DateRealisationProjet { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.Province> Province { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.Region> Region { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.District> District { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.Commune> Commune { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.Partenaire> Partenaire { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.SousActivite> SousActivite { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.Composant> Composant { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.Resultat> Resultat { get; set; } = default!;
    public DbSet<AnaeLogiciel.Models.Bailleur> Bailleur { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.ProjetComposant> ProjetComposant { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceResultat> OccurenceResultat { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceActivite> OccurenceActivite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceSousActivite> OccurenceSousActivite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.SiteActivite> SiteActivite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.SiteSousActivite> SiteSousActivite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceActiviteIndicateur> OccurenceActiviteIndicateur { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceSousActiviteIndicateur> OccurenceSousActiviteIndicateur { get; set; } = default!;

    public DbSet<AnaeLogiciel.Models.IndicateurTechnicienSiteSousActivite> IndicateurTechnicienSiteSousActivite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.IndicateurTechnicienSiteActivite> IndicateurTechnicienSiteActivite { get; set; } = default!;

    public DbSet<AnaeLogiciel.Models.TechnicienProjet> TechnicienProjet { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.RapportIndicateurActivite> RapportIndicateurActivite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.RapportIndicateurSousActivite> RapportIndicateurSousActivite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.VCalculRapportActivite> VCalculRapportActivite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.VCalculRapportSousActivite> VCalculRapportSousActivite { get; set; } = default!;

    public DbSet<AnaeLogiciel.Models.VAvancementResultat> VAvancementResultat { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.VAvancementProjet> VAvancementProjet { get; set; } = default!;

    public DbSet<AnaeLogiciel.Models.PaiementOccurenceSousActivite> PaiementOccurenceSousActivite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.PaiementOccurenceActivite> PaiementOccurenceActivite { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceActiviteSource> OccurenceActiviteSource { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceSousActiviteSource> OccurenceSousActiviteSource { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceActiviteSourceTechnicien> OccurenceActiviteSourceTechnicien { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.OccurenceSousActiviteSourceTechnicien> OccurenceSousActiviteSourceTechnicien { get; set; } = default!;

    public DbSet<AnaeLogiciel.Models.VLienActiviteSousActivite> VLienActiviteSousActivite { get; set; } = default!;

    public DbSet<AnaeLogiciel.Models.Devise> Devise { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.PartenaireTechnique> PartenaireTechnique { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.ProjetPartenaireTechnique> ProjetPartenaireTechnique { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.PartiePrenante> Partieprenante { get; set; } = default!;
    
    public DbSet<AnaeLogiciel.Models.PartiePrenanteOccurenceActivite> PartiePrenanteOccurenceActivite { get; set; } = default!;

    
    public DbSet<AnaeLogiciel.Models.ProlongementBudgetProjet> ProlongementBudgetProjet { get; set; } = default!;
    

    public DbSet<AnaeLogiciel.Models.ProlongementProjet> ProlongementProjet { get; set; } = default!;
    
    
    public DbSet<AnaeLogiciel.Models.ProlongementBudgetOccurenceActivite> ProlongementBudgetOccurenceActivite { get; set; } = default!;
    
    
    public DbSet<AnaeLogiciel.Models.ProlongementOccurenceActivite> ProlongementOccurenceActivite { get; set; } = default!;

    public DbSet<AnaeLogiciel.Models.VCalculStatHommeFemme> VCalculStatHommeFemme { get; set; } = default!;
}