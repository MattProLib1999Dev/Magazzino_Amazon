namespace Amazon;

public class AddProdottoOutPutDto
{
    public AddProdottoOutPutDto() {

    }

    public int IdDelProdotto = 0;
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

    public int Prezzo = 0;
}
