using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnaeLogiciel.Models.Interface;

namespace AnaeLogiciel.Models;

[Table("region")]
public class Region
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("id_province")]
    [DisplayName("province")]
    public int IdProvince { get; set; }
    
    [Column("nom")]
    public string Nom { get; set; }
    
    [ForeignKey("IdProvince")]
    public virtual Province? Province { get; set; }
}