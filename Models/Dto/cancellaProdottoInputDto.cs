namespace Amazon;

public class CancellaProdottoInput
{
     public CancellaProdottoInput()
     {
        
     }

    public int IdDelProdotto { get; set; }
    public string? Nome
    {
        get;
        set;
    } = String.Empty;
    public string? Indirizzo
    {
        get;
        set;
    } = String.Empty;
    public string? Provenienza
    {
        get;
        set;
    } = String.Empty;
    public string? Citta
    {
        get;
        set;
    } = String.Empty;

    public int? Prezzo { get; set; }
}
