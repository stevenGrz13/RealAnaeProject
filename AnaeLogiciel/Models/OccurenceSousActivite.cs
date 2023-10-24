using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("occurencesousactivite")]
public class OccurenceSousActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurenceactivite")]
    [DisplayName("occurenceactivite")]
    public int IdOccurenceActivite { get; set; }
    
    [NotMapped]
    public string NomSousActivite { get; set; }
    
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
    
    [ForeignKey("IdOccurenceActivite")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
}