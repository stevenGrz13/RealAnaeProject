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
    
    [Column("idprojet")]
    [DisplayName("projet")]
    public int IdProjet { get; set; }
    
    [Column("idactivite")]
    [DisplayName("activite")]
    public int IdActivite { get; set; }
    
    [Column("datedebutprevision")]
    public DateOnly DateDebutPrevision { get; set; }
    
    [Column("datefinprevision")]
    public DateOnly DateFinPrevision { get; set; }
    
    [Column("budget")]
    public double Budget { get; set; }
    
    [Column("finishedornot")]
    public bool FinishedOrNot { get; set; }
    
    [Column("avancement")]
    public double Avancement { get; set; }
    
    [ForeignKey("IdProjet")]
    public virtual Projet? Projet { get; set; }
    
    [ForeignKey("IdActivite")]
    public virtual Activite? Activite { get; set; }
}