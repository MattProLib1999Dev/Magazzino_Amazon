using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amazon.Prodotto
{
    public class Prodotto { 

    public Prodotto()
    {
            
    }

    [Key , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("foreignKeyProdottoUserModelResponse")]
    public int foreignKeyProdottoUserModelResponseId { get; set; }

    [MaxLength(20)]
    public string Descrizione { get; set; } = String.Empty;
    

    [MaxLength(20)]
    public string Nome { get; set; } = String.Empty;

    [MaxLength(20)]
    public float Prezzo { get; set; }

    
    [MaxLength(20)]
    public string Indirizzo
    {
        get;
        set;
    } = String.Empty;

    [MaxLength(20)]
    public string Provenienza
    {
        get;
        set;
    } = String.Empty;

    [MaxLength(20)]
    public string Citta
    {
        get;
        set;
    } = String.Empty;
}

}

