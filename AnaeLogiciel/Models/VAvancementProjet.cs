using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AnaeLogiciel.Models;

[Table("vavancementprojet")]
[Keyless]
public class VAvancementProjet
{
    [Column("idprojet")]
    [DisplayName("projet")]
    public int IdProjet { get; set; }
    
    [Column("avancement")]
    public double Avancement { get; set; }
    
    [ForeignKey("IdProjet")]
    public virtual Projet? Projet { get; set; }
}