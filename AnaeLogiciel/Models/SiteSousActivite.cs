using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AnaeLogiciel.Models;

[Table("sitesousactivite")]
public class SiteSousActivite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idoccurencesousactivite")]
    [DisplayName("occurencesousactivite")]
    public int IdOccurenceSousActivite { get; set; }
    
    [Column("libelle")]
    public string Libelle { get; set; }
    
    [Column("idcommune")]
    [DisplayName("commune")]
    public int IdCommune { get; set; }
    
    [Column("iddistrict")]
    [DisplayName("district")]
    public int IdDistrict { get; set; }
    
    [Column("idregion")]
    [DisplayName("region")]
    public int IdRegion { get; set; }
    
    [ForeignKey("IdOccurenceSousActivite")]
    public virtual OccurenceSousActivite? OccurenceSousActivite { get; set; }
    
    [ForeignKey("IdCommune")]
    public virtual Commune? Commune { get; set; }
    
    [ForeignKey("IdDistrict")]
    public virtual District? District { get; set; }
    
    [ForeignKey("IdRegion")]
    public virtual Region? Region { get; set; }
}