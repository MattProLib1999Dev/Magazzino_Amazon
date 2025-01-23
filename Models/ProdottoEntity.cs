using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amazon
{
    [Table("Prodotto")]
    public class ProdottoEntity
    {
        public ProdottoEntity()
        {
        }
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(20)]
        public int id { get; set; }     

        [MaxLength(20)]
        public string Descrizione { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Nome { get; set; } = string.Empty;

        public float Prezzo { get; set; }

        [MaxLength(20)]
        public string Indirizzo { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Provenienza { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Citta { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Url { get; set; } = string.Empty;
    }
}
