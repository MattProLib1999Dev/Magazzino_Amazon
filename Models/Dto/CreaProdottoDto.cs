namespace Amazon;

public class CreaProdottoInputDto
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

    public float Prezzo { get; set; }

    public string Url { get; set; } = String.Empty;

}
