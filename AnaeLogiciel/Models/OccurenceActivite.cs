using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("occurenceactivite")]
public class OccurenceActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurenceresultat")]
    [DisplayName("occurenceresultat")]
    public int IdOccurenceResultat { get; set; }
    
    [NotMapped]
    public string NomActivite { get; set; }
    
    [Column("budget")]
    public double Budget { get; set; }
    
    [Column("datedebut")]
    public DateOnly DateDebut { get; set; }
    
    [Column("datefin")]
    public DateOnly DateFin { get; set; }
    
    [Column("avancement")]
    public double Avancement { get; set; }
    
    [Column("details")]
    public string Details { get; set; }
    
    [Column("issupp")]
    public bool IsSupp { get; set; }
    
    [NotMapped]
    public string Couleur { get; set; }
    
    [NotMapped]
    public string Message { get; set; }
    
    [ForeignKey("IdOccurenceResultat")]
    public virtual OccurenceResultat? OccurenceResultat { get; set; }
    
    [NotMapped]
    public List<OccurenceSousActivite> ListeOccurenceSousActivites { get; set; }
}