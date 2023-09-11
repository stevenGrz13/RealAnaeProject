using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("viewsite")]
[Keyless]
public class ViewSite
{
    [Column("idactivite")]
    [DisplayName("activite")]
    public int IdActivite { get; set; }
    
    [Column("idprojet")]
    [DisplayName("projet")]
    public int IdProjet { get; set; }
    
    [Column("idoccurence")]
    [DisplayName("occurenceactivite")]
    public int IdOccurence { get; set; }

    [Column("idtypeindicateur")]
    [DisplayName("typeindicateur")]
    public int IdTypeIndicateur { get; set; }

    [Column("quantitedemande")]
    public double QuantiteDemande { get; set; }
    
    [ForeignKey("IdActivite")]
    public virtual Activite? Activite { get; set; }
    
    [ForeignKey("IdProjet")]
    public virtual Projet? Projet { get; set; }
    
    [ForeignKey("IdOccurence")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdTypeIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
}