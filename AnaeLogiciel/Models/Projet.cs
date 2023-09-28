using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaeLogiciel.Models;

[Table("projet")]
public class Projet
{
    [Key] 
    [Column("id")]
    public int Id { get; set; }
    
    [Column("nom")]
    public string Nom { get; set; }
    
    [Column("datedebutprevision")]
    public DateOnly DateDebutPrevision { get; set; }
    
    [Column("datefinprevision")]
    public DateOnly DateFinPrevision { get; set; }
    
    [Column("finishedornot")]
    public bool FinishedOrNot { get; set; }
    
    [Column("avancement")]
    public double Avancement { get; set; }
    
    [Column("details")]
    public string Details { get; set; }
    
    [Column("iddevise")]
    [DisplayName("devise")]
    public int IdDevise { get; set; }
    
    [Column("idbailleur")]
    [DisplayName("bailleur")]
    public int IdBailleur { get; set; }
    
    [Column("budget")]
    public double Budget { get; set; }
    
    [Column("sigle")]
    public string Sigle { get; set; }
    
    [Column("reference")]
    public int Reference { get; set; }
    
    [ForeignKey("IdBailleur")]
    public virtual Bailleur? Bailleur { get; set; }
    
    [ForeignKey("IdDevise")]
    public virtual Devise? Devise { get; set; }
}