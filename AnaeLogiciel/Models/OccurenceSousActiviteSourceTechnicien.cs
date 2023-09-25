using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("occurencesousactivitesourcetechnicien")]
public class OccurenceSousActiviteSourceTechnicien
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurencesousactivite")]
    [DisplayName("occurencesousactivite")]
    public int IdOccurenceSousActivite { get; set; }
    
    [Column("idsource")]
    [DisplayName("sourcedeverification")]
    public int IdSource { get; set; }
    
    [Column("idtechnicien")]
    [DisplayName("technicien")]
    public int IdTechnicien { get; set; }
    
    [Column("lienfichier")]
    public string LienFichier { get; set; }
    
    [Column("dateaction")]
    public DateOnly DateAction { get; set; }
    
    [ForeignKey("IdOccurenceSousActivite")]
    public virtual OccurenceSousActivite? OccurenceSousActivite { get; set; }
    
    [ForeignKey("IdSource")]
    public virtual SourceDeVerification? SourceDeVerification { get; set; }
    
    [ForeignKey("IdTechnicien")]
    public virtual Technicien? Technicien { get; set; }
}