using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("vcalculavancement")]
[Keyless]
public class AssociationIndicateurActiviteAvecDate
{
    [Column("idoccurence")]
    [DisplayName("occurenceactivite")]
    public int IdOccurence { get; set; }
    
    [Column("idindicateur")]
    [DisplayName("typeindicateur")]
    public int IdTypeIndicateur { get; set; }
    
    [Column("targeteffectue")]
    public double TargetEffectue { get; set; }
    
    [ForeignKey("IdTypeIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
    
    [ForeignKey("IdOccurence")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
}