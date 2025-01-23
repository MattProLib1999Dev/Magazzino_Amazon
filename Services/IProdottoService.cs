using Amazon;
using Amazon.Controllers;
using Amazon.Prodotto;

public interface IProdottoService
{
    public List<AddProdottoDtoInput> GetProdotti();
    public ProdottoDtoInput GetProdotto(int numeroProdotto);
    public bool CancellaUnProdotto(int idDelProdotto);
    public CreaProdottoInputDto CreaProdotto(CreaProdottoInputDto creaProdottoInputDto);
    public UpdateProdottoDtoInput UpdateProdotto(int IdDelProdotto, UpdateProdottoDtoInput prodotto);
    public AddProdottoDtoInput addProdotto(int idDelProdotto, AddProdottoDtoInput addProdotto);
    public List<ProdottoDtoInput> GetListProdotti();
     public int RestituiscimiLaQuantita();
    public int ModificaLaQuantitàDeiProdotti(int quantita);
    public InviaUnProdottoDto InviaUnProdotto();
    object GetNumeroProdottiCreati();
}

public class ProdottoService : IProdottoService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ProdottoService> _logger;

    public ProdottoService(ApplicationDbContext dbContext, ILogger<ProdottoService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<CreaProdottoInputDto> RestituiscimiUnaListaDiProdotti()
    {
        return new List<CreaProdottoInputDto>();
    }
    public bool CancellaUnProdotto(int idDelProdotto)
    {
        return true;
    }
    public InviaUnProdottoDto InviaUnProdotto()
    {
        return new InviaUnProdottoDto();
    }
    /* public List<AddProdottoDtoInput> GetListProdotti()
    {
        return new List<AddProdottoDtoInput>();
    } */
    public ProdottoDto RestituiscimiUnSoloProdotto()
    {
        return new ProdottoDto();
    }

    public CreaProdottoInputDto CreaProdotto(CreaProdottoInputDto creaProdottoInputDto)
    {
        return new CreaProdottoInputDto();
    }
    public UpdateProdottoDtoInput UpdateProdotto(int IdDelProdotto, UpdateProdottoDtoInput prodotto)
    {
        return new UpdateProdottoDtoInput();
    }
    public ProdottoDtoInput GetProdottoDtoInput()
    {
        return new ProdottoDtoInput();
    }

    public ProdottoDtoInput GetProdotto(int numeroProdotto)
    {
        return new ProdottoDtoInput();
    }

    public AddProdottoDto addProdotto(int idDelProdotto, AddProdotto addProdotto)
    {
        return new AddProdottoDto();
    }

    public AddProdottoDtoInput addProdotto(int idDelProdotto, AddProdottoDtoInput addProdotto)
    {
        return new AddProdottoDtoInput();
    }

    private List<ProdottoDtoInput> GetProdottiMock()
    {
        return new List<ProdottoDtoInput>
        {
            new ProdottoDtoInput { IdDelProdotto = 1, Citta = "Roma", Nome = "Prodotto 1", Indirizzo = "via dell'Elcino 2", Provenienza = "Europa", Prezzo = 10 },
            new ProdottoDtoInput { IdDelProdotto = 2, Citta = "Roma",Nome = "Prodotto 2",Indirizzo = "via dell'Elcino 2" ,Provenienza = "Europa", Prezzo = 15 },
            new ProdottoDtoInput { IdDelProdotto = 3, Citta = "Roma", Indirizzo = "via dell'Elcino 2",Nome = "Prodotto 3",Provenienza = "Europa", Prezzo = 20 }
        };
    }

        public List<ProdottoDtoInput> GetListProdotti()
        {
            var prodotti = new List<ProdottoDtoInput>();

            try
            {
                if (_dbContext == null)
                {
                    _logger.LogWarning("Il contesto del database è null. Restituisco dati simulati.");

                    // Dati simulati nel caso in cui _dbContext sia null
                    prodotti = GetProdottiMock().Select(p => MappaProdottoADto(p)).ToList();
                    return prodotti;
                }

                if (_dbContext.ListaDeiProdotti == null)
                {
                    _logger.LogWarning("La tabella ListaDeiProdotti è null. Restituisco dati simulati.");

                    // Dati simulati nel caso in cui ListaDeiProdotti sia null
                    prodotti = GetProdottiMock().Select(p => MappaProdottoADto(p)).ToList();
                    return prodotti;
                }

                // Recupero i prodotti dal database e mappo i risultati in ProdottoDtoInput
                prodotti = _dbContext.ListaDeiProdotti
                    .Select(p => MappaProdottoADto(p))
                    .ToList();

                // Se la tabella è vuota, popolala con dati simulati
                if (prodotti == null || !prodotti.Any())
                {
                    _logger.LogInformation("La tabella ListaDeiProdotti è vuota. Aggiungo dati simulati.");
                    prodotti = GetProdottiMock().Select(p => MappaProdottoADto(p)).ToList();

                    // Salvo i dati simulati nel database per test futuri
                    _dbContext.ListaDeiProdotti.AddRange((IEnumerable<Prodotto>)prodotti);
                    _dbContext.SaveChanges();
                }

                _logger.LogInformation("Numero di prodotti trovati: {Count}", prodotti.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero della lista dei prodotti. Restituisco dati simulati.");

                // Dati simulati in caso di eccezione
                prodotti = GetProdottiMock().Select(p => MappaProdottoADto(p)).ToList();
            }

            return prodotti;

        }


    // Metodo di mappatura da Prodotto a ProdottoDtoInput
    private ProdottoDtoInput MappaProdottoADto(ProdottoDtoInput prodotto)
    {
        return new ProdottoDtoInput
        {
            // Mappatura delle proprietà tra Prodotto e ProdottoDtoInput
            IdDelProdotto = prodotto.IdDelProdotto,
            Nome = prodotto.Nome,
            Prezzo = prodotto.Prezzo
            // Aggiungi altre proprietà se necessario
        };
    }
        

    // Metodo di mappatura da Prodotto a ProdottoDtoInput
    private ProdottoDtoInput MappaProdottoADto(Prodotto prodotto)
    {
        return new ProdottoDtoInput
        {
            IdDelProdotto = prodotto.Id,
            Nome = prodotto.Nome,
            Prezzo = prodotto.Prezzo
        };
    }


    // Metodo per ottenere i dati simulati (mock)
    private List<Prodotto> GetProdottoMock()
    {
        return new List<Prodotto>
        {
            new Prodotto { Id = 1, Nome = "Prodotto 1", Prezzo = 10 },
            new Prodotto { Id = 2, Nome = "Prodotto 2", Prezzo = 15 },
            new Prodotto { Id = 3, Nome = "Prodotto 3", Prezzo = 7 }
        };
    }
    public List<AddProdottoDtoInput> GetProdotti()
    {
        throw new NotImplementedException();
    }
    
    public int RestituiscimiLaQuantita()
{
    try
    {
        // Utilizza il metodo GetListProdotti per ottenere la lista dei prodotti
        var prodotti = GetListProdotti();

        // Restituisce la quantità degli elementi
        _logger.LogInformation("Numero di prodotti calcolato: {Count}", prodotti.Count);
        return prodotti.Count;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Errore durante il calcolo della quantità dei prodotti.");
        return 0; // In caso di errore, restituisci 0
    }
}


    public int ModificaLaQuantitàDeiProdotti(int quantita)
    {
        throw new NotImplementedException();
    }

    public object GetNumeroProdottiCreati()
    {
        throw new NotImplementedException();
    }


}