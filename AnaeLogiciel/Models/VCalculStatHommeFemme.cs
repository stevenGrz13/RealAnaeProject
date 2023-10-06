using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("vcalculstathommefemme")]
[Keyless]
public class VCalculStatHommeFemme
{
    [Column("idprojet")]
    [DisplayName("projet")]
    public int IdProjet { get; set; }
    
    [Column("idindicateur")]
    [DisplayName("typeindicateur")]
    public int IdIndicateur { get; set; }
    
    [Column("quantite")]
    public double Quantite { get; set; }
    
    [ForeignKey("IdProjet")]
    public virtual Projet? Projet { get; set; }
    
    [ForeignKey("IdIndicateur")]
    public virtual TypeIndicateur? TypeIndicateur { get; set; }
}