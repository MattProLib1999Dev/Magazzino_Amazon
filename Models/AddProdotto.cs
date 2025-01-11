using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amazon;

[Table("AddProdotto")]
public class AddProdotto
{
    [Column("Descrizione",Order = 1, TypeName = "String")]
    [MaxLength(20)]
    public string Descrizione { get; set; } = String.Empty;
    

    [Column("Nome",Order = 1, TypeName = "String")]
    [MaxLength(20)]
    public string Nome { get; set; } = String.Empty;

    [Column("Prezzo",Order = 1, TypeName = "Float")]
    [MaxLength(20)]
    public required float Prezzo { get; set; }

    [Column("Indirizzo",Order = 1, TypeName = "Indirizzo")]
    
    [MaxLength(20)]
    public string Indirizzo
    {
        get;
        set;
    } = String.Empty;

    [Column("Provenienza",Order = 1, TypeName = "String")]
    [MaxLength(20)]
    public string Provenienza
    {
        get;
        set;
    } = String.Empty;

    [Column("Citta",Order = 1, TypeName = "String")]
    [MaxLength(20)]
    public string Citta
    {
        get;
        set;
    } = String.Empty;

    public string Url { get; set; } = String.Empty;
}
