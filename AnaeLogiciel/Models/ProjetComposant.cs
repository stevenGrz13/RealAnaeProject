using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("projetcomposant")]
public class ProjetComposant
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idprojet")]
    [DisplayName("projet")]
    public int IdProjet { get; set; }
    
    [Column("idcomposant")]
    [DisplayName("composant")]
    public int IdComposant { get; set; }

    [ForeignKey("IdProjet")] public virtual Projet? Projet { get; set; }
    
    [ForeignKey("IdComposant")] public virtual Composant? Composant { get; set; }
}