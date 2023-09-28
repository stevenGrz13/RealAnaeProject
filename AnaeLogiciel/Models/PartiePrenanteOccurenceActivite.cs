using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("partieprenanteoccurenceactivite")]
public class PartiePrenanteOccurenceActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurenceactivite")]
    [DisplayName("occurenceactivite")]
    public int IdOccurenceActivite { get; set; }
    
    [Column("idpartieprenante")]
    [DisplayName("partieprenante")]
    public int IdPartiePrenante { get; set; }
    
    [ForeignKey("IdOccurenceActivite")]
    public virtual OccurenceActivite? OccurenceActivite { get; set; }
    
    [ForeignKey("IdPartiePrenante")]
    public virtual PartiePrenante? PartiePrenante { get; set; }
}