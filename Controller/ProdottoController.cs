using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Amazon;

[Route("[controller]")]
[ApiController]

public class ProdottiController(IConfiguration configuration, IProdottiRepository IProdottoRepository, ApplicationDbContext applicationDbContext, IProdottoService IProdottoService) : ControllerBase
{
    private readonly List<ProdottoEntity> _prodotti = new List<ProdottoEntity>();
    private readonly ILogger<ProdottiController>? inputLogger;
    private readonly ApplicationDbContext dbContext = new ApplicationDbContext();
    IConfiguration _configuration = configuration;
    private readonly IProdottiRepository _Irepository = IProdottoRepository;
    private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
    private readonly FakeDatabase? database;

    public record ProdottiControllerRecord(ILogger InputLogger, FakeDatabase Database)
    {
        public ProdottiControllerRecord(ILogger inputLogger, ILogger<FakeDatabase> fakeDbLogger)
            : this(inputLogger, new FakeDatabase(fakeDbLogger))
        {
        }
    }


    [EnableCors("AnotherPolicy")]
    [HttpDelete("CancellaUnProdotto")]
    public ActionResult<cancellaProdottoInputDto> CancellaUnProdotto([FromBody] cancellaProdottoInputDto cancellaProdotto, [FromRoute] int IdDelProdotto)
    {

        var cancellaProdottoInputDto = new cancellaProdottoInputDto()
        {
            Citta = cancellaProdotto.Citta,
            Indirizzo = cancellaProdotto.Indirizzo,
            Nome = cancellaProdotto.Nome,
            Prezzo = cancellaProdotto.Prezzo,
            Provenienza = cancellaProdotto.Provenienza
        };

        IProdottoService.CancellaUnProdotto();
        dbContext.SaveChanges();
        return Ok(IProdottoService.CancellaUnProdotto());
    }

    [EnableCors("AnotherPolicy")]
    [HttpPost("CreaUnProdotto")]
    public IActionResult RestituiscimiLaListaDeiProdotti([FromQuery] string citta, [FromBody] CreaProdottoDto creaProdottoInputDto)
    {
        var prodottoEntity = new CreaProdottoInputDto()
        {
            Citta = creaProdottoInputDto.Citta,
            Indirizzo = creaProdottoInputDto.Indirizzo,
            Nome = creaProdottoInputDto.Nome,
            Prezzo = creaProdottoInputDto.Prezzo,
            Provenienza = creaProdottoInputDto.Provenienza
        };

        IProdottoService.CreaProdotto();
        dbContext.SaveChanges();
        return Ok(IProdottoService.CreaProdotto() ?? throw new Exception("il prodotto non è stato creato"));

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
    public ActionResult<int> ReStituiscimiLaQuantitàDeiProdotti([FromRoute] int quantita)
    {
        int quantitàDeiProdotti = 0;
        dbContext.SaveChanges();
        IProdottoService.RestituiscimiLaQuantita();
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

