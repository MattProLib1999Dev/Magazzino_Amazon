using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amazon;

[Table("Titolare")]
public class Titolare
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    [Column("Nome",Order = 1, TypeName = "Nome")]
    [MaxLength(20)]
    public string? Nome { get; set; }

    [Column("Cognome",Order = 1, TypeName = "Cognome")]
    [MaxLength(20)]
    public string? Cognome { get; set; }
}
