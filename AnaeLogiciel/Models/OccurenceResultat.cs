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
    
    [Column("nomresultat")]
    public string NomResultat { get; set; }
    
    [Column("avancement")]
    public double Avancement { get; set; }
    
    [Column("issupp")]
    public bool IsSupp { get; set; }
    
    [ForeignKey("IdProjet")]
    public virtual Projet? Projet { get; set; }
    
    [NotMapped]
    public List<OccurenceActivite> ListeOccurenceActivites { get; set; }
}