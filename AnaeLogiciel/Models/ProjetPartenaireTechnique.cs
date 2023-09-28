using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("projetpartenairetechnique")]
public class ProjetPartenaireTechnique
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idprojet")]
    [DisplayName("projet")]
    public int IdProjet { get; set; }
    
    [Column("idpartenairetechnique")]
    [DisplayName("partenairetechnique")]
    public int IdPartenaireTechnique { get; set; }
    
    [ForeignKey("IdProjet")]
    public virtual Projet? Projet { get; set; }
    
    [ForeignKey("IdPartenaireTechnique")]
    public virtual PartenaireTechnique? PartenaireTechnique { get; set; }
}