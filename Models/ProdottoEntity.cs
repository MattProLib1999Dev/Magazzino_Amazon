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

        [Column("Descrizione", Order = 1, TypeName = "String")]
        [MaxLength(20)]
        public int id { get; set; }     

        [Column("Descrizione", Order = 1, TypeName = "String")]
        [MaxLength(20)]
        public string Descrizione { get; set; } = string.Empty;

        [Column("Nome", Order = 1, TypeName = "String")]
        [MaxLength(20)]
        public string Nome { get; set; } = string.Empty;

        [Column("Prezzo", Order = 1, TypeName = "Float")]
        public float Prezzo { get; set; }

        [Column("Indirizzo", Order = 1, TypeName = "String")]
        [MaxLength(20)]
        public string Indirizzo { get; set; } = string.Empty;

        [Column("Provenienza", Order = 1, TypeName = "String")]
        [MaxLength(20)]
        public string Provenienza { get; set; } = string.Empty;

        [Column("Citta", Order = 1, TypeName = "String")]
        [MaxLength(20)]
        public string Citta { get; set; } = string.Empty;

        [Column("Citta", Order = 1, TypeName = "String")]
        [MaxLength(20)]
        public string Url { get; set; } = string.Empty;
    }
}
