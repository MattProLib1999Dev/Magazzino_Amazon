using Microsoft.EntityFrameworkCore;

namespace Amazon;

[PrimaryKey(nameof(IdDelProdotto))]
public class CreaProdottoDto
{
    public int IdDelProdotto { get; set; }
    public string Nome
    {
        get;
        set;
    } = String.Empty;
    public string Indirizzo
    {
        get;
        set;
    } = String.Empty;
    public string Provenienza
    {
        get;
        set;
    } = String.Empty;
    public string Citta
    {
        get;
        set;
    } = String.Empty;

    public int Prezzo { get; set; }

}
