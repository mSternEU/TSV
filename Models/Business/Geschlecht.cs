using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSV.Models.Base;

[Table("geschlecht")]
public partial class Geschlecht : ModelBase
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("bezeichnung")]
    public string Bezeichnung { get; set; }
}