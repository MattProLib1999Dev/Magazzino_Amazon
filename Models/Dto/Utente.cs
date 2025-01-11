using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amazon;

public class Utente
{

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUtente { get; set; }
    public string nome { get; set; } = String.Empty;

    public string cognome { get; set; } = String.Empty;

    public string userName { get; set; } = String.Empty;

    public string password { get; set; } = String.Empty;
}
