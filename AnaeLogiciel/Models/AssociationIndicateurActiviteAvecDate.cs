using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("rapporttechniciensite")]
public class AssociationIndicateurActiviteAvecDate
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurence")]
    [DisplayName("occurenceactivite")]
    public int IdOccurence { get; set; }
    
    [Column("idindicateur")]
    [DisplayName("typeindicateur")]
    public int IdTypeIndicateur { get; set; }
    
    [Column("idsite")]
    [DisplayName("site")]
    public int IdSite { get; set; }
    
    [Column("dateaction")]
    public DateOnly Dateaction { get; set; }
    
    [Column("targeteffectue")]
    public double TargetEffectue { get; set; }
    
    [ForeignKey("IdSite")]
    public virtual Site? Site { get; set; }
    
    [ForeignKey("IdTypeIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
    
    [ForeignKey("IdOccurence")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
}