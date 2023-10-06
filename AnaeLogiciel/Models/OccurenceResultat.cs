using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("occurenceresultat")]
public class OccurenceResultat
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idprojet")]
    [DisplayName("projet")]
    public int IdProjet { get; set; }
    
    [Column("idresultat")]
    [DisplayName("resultat")]
    public int IdResultat { get; set; }
    
    [NotMapped] public string NomResultat { get; set; }
    
    [Column("avancement")]
    public double Avancement { get; set; }
    
    [ForeignKey("IdProjet")]
    public virtual Projet? Projet { get; set; }
    
    [ForeignKey("IdResultat")]
    public virtual Resultat? Resultat { get; set; }
    
    [NotMapped]
    public List<OccurenceActivite> ListeOccurenceActivites { get; set; }
}