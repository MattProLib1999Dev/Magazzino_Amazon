using System.Data;
using Amazon.DAL.Handlers.PasswordHasher.Abstract;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amazon;

[Route("[controller]")]
[ApiController]

public class ProdottiController(IConfiguration configuration, IProdottiRepository IProdottoRepository, ApplicationDbContext applicationDbContext, IProdottoService IProdottoService) : ControllerBase
{
    private readonly List<ProdottoEntity> _prodotti = new List<ProdottoEntity>();
    private readonly ILogger<ProdottiController>? inputLogger;
    private readonly ApplicationDbContext dbContext = new ApplicationDbContext();
    IConfiguration _configuration = configuration;
    private ILogger<AccountHandlers>? _logger;
    private readonly IProdottiRepository _Irepository = IProdottoRepository;
    private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
    private readonly FakeDatabase? database;
    private readonly DbContext? _dbContext;

    public record ProdottiControllerRecord(ILogger InputLogger, FakeDatabase Database)
    {
        private readonly DbContext? _dbContext;

        public ProdottiControllerRecord(ILogger inputLogger, ILogger<FakeDatabase> fakeDbLogger, IDevelopparePassworHasher passworHasher, DbContext dbContext)
            : this(inputLogger, new FakeDatabase(fakeDbLogger, passworHasher))
        {
                this._dbContext = dbContext;

        }
    }

[EnableCors("AnotherPolicy")]
[HttpDelete("CancellaUnProdotto/{IdDelProdotto}")]
public IActionResult CancellaUnProdotto([FromRoute] int IdDelProdotto)
{
    try
    {
        // Usa il servizio per cancellare il prodotto
        var prodottoCancellato = IProdottoService.CancellaUnProdotto(IdDelProdotto);

        // Se il prodotto non è stato trovato o cancellato, restituisci un errore
        if (prodottoCancellato == null)
        {
            return NotFound("Prodotto non trovato.");
        }

        // Restituisci un messaggio di successo se il prodotto è stato cancellato correttamente
        return Ok(new { message = "Prodotto cancellato con successo." });
    }
    catch (Exception ex)
    {
        // Gestione degli errori in caso di problemi durante la cancellazione
        _logger.LogError(ex, "Errore durante la cancellazione del prodotto.");
        return StatusCode(500, "Si è verificato un errore durante la cancellazione del prodotto.");
    }
}







    [EnableCors("AnotherPolicy")]
    [HttpPost("CreaUnProdotto")]
public IActionResult RestituiscimiLaListaDeiProdotti([FromQuery] string citta, [FromBody] CreaProdottoDto creaProdottoInputDto)
{
    try
    {
        var prodottoEntity = new CreaProdottoInputDto
        {
            Citta = creaProdottoInputDto.Citta,
            Indirizzo = creaProdottoInputDto.Indirizzo,
            Nome = creaProdottoInputDto.Nome,
            Prezzo = creaProdottoInputDto.Prezzo,
            Provenienza = creaProdottoInputDto.Provenienza
        };

        // Usa il servizio per creare il prodotto
        var prodottoCreato = IProdottoService.CreaProdotto(prodottoEntity);

        if (prodottoCreato == null)
        {
            return BadRequest("Il prodotto non è stato creato.");
        }

        return Ok(prodottoCreato);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Errore durante la creazione del prodotto.");
        return StatusCode(500, "Si è verificato un errore durante la creazione del prodotto.");
    }
}


    [EnableCors("AnotherPolicy")]
    [HttpPut("ModificaIlNomeDelProdotto/{idProdotto}")]
    public ActionResult<ProdottoEntity> ModificaIlNomeDelProdotto(int idDelProdotto, [FromBody] UpdateProdottoDtoInput modificaIlNomeDelProdottoDtoInput)
    {
        var prodottoEntity = new UpdateProdottoDtoInput()
        {
            Citta = modificaIlNomeDelProdottoDtoInput.Citta,
            Indirizzo = modificaIlNomeDelProdottoDtoInput.Indirizzo,
            Nome = modificaIlNomeDelProdottoDtoInput.Nome,
            Prezzo = modificaIlNomeDelProdottoDtoInput.Prezzo,
            Provenienza = modificaIlNomeDelProdottoDtoInput.Provenienza
        };
        IProdottoService.UpdateProdotto(idDelProdotto, prodottoEntity);
        dbContext.SaveChanges();
        return Ok(IProdottoService.UpdateProdotto(idDelProdotto, prodottoEntity) ?? throw new Exception("il prodotto non è stato modificato correttamente"));
    }

    [EnableCors("AnotherPolicy")]
    [HttpGet("RestituiscimiUnProdotto/{idProdotto}")]
    public ActionResult<ProdottoEntity> RestituiscimiUnProdotto(int idDelProdotto, [FromBody] UpdateProdottoDtoInput addProdotto)
    {
        var prodottoEntity = IProdottoService.GetProdotto(idDelProdotto);
        if (prodottoEntity == null)
        {
            return NotFound();
        }

        var prodottoDto = new ProdottoEntity
        {
            Citta = prodottoEntity.Citta,
            Indirizzo = prodottoEntity.Indirizzo,
            Nome = prodottoEntity.Nome,
            Prezzo = prodottoEntity.Prezzo,
            Provenienza = prodottoEntity.Provenienza
        };

        return Ok(prodottoDto) ?? throw new Exception("il prodotto è stato restituito correttamente");
    }

    [EnableCors("AnotherPolicy")]
    [HttpGet("RestituiscimiLaListaDeiProdotti")]
    public ActionResult<List<ProdottoDto>> RestituiscimiLaListaDeiProdotti([FromBody] ProdottoDto prodotti)
    {
        var Prodotto = new ProdottoEntity()
        {
            Citta = prodotti.Citta,
            Indirizzo = prodotti.Citta,
            Nome = prodotti.Citta,
            Prezzo = prodotti.Prezzo,
            Provenienza = prodotti.Provenienza
        };
        dbContext.Add(Prodotto);
        dbContext.SaveChanges();
        return Ok(IProdottoService.GetListProdotti() ?? throw new Exception("il prodotto è stato restituito correttamente"));
    }

    [EnableCors("AnotherPolicy")]
    [HttpGet("RestituiscimiLaQuantitàDeiProdotti")]
    public ActionResult<int> ReStituiscimiLaQuantitàDeiProdotti(List<ProdottoDtoInput> listProdottoDto)
    {
        int quantitàDeiProdotti = 0;
        dbContext.SaveChanges();
        IProdottoService.RestituiscimiLaQuantita(listProdottoDto);
        return Ok(quantitàDeiProdotti) ?? throw new Exception("la quantità dei prodotti è restituita correttamente");
    }

    [EnableCors("AnotherPolicy")]
    [HttpPut("ModificaLaQuantitàDeiProdotti")]
    public ActionResult<int> ModificaLaQuantitàDeiProdotti([FromRoute] int quantita)
    {
        int quantitàDeiProdotti = 0;
        IProdottoService.ModificaLaQuantitàDeiProdotti();
        dbContext.SaveChanges();
        return Ok(quantitàDeiProdotti) ?? throw new Exception("la modifica del prodotto è stata eseguita correttamente");
    }

    [EnableCors("AnotherCors")]
    [HttpPost("InviaLaQuantitaDeiProdotti")]
    public IActionResult RestituiscimiLaListaDeiProdotti([FromQuery] string citta, [FromBody] ProdottoDto prodotto)
    {
        int quantità = 0;
        IProdottoService.InviaLaQuantitàDeiProdotti();
        return Ok(quantità) ?? throw new Exception("la quantita' dei prodotti è stata inviata correttamente");
    }








}

