using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Amazon.DAL.Handlers.PasswordHasher.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Amazon.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdottiController : ControllerBase
    {
        private readonly IProdottoService _prodottoService;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProdottiController> _logger;

        public ProdottiController(
            IConfiguration configuration, 
            IProdottiRepository prodottoRepository, 
            ApplicationDbContext dbContext, 
            IProdottoService prodottoService,
            ILogger<ProdottiController> logger)
        {
            _prodottoService = prodottoService ?? throw new ArgumentNullException(nameof(prodottoService));
            _dbContext = dbContext;
            _logger = logger;
        }

        [EnableCors("AnotherPolicy")]
        [HttpDelete("CancellaUnProdotto/{idDelProdotto}")]
        public IActionResult CancellaUnProdotto([FromRoute] int idDelProdotto)
        {
            var prodottoCancellato = _prodottoService.CancellaUnProdotto(idDelProdotto);

            if (prodottoCancellato == null)
            {
                return NotFound("Prodotto non trovato.");
            }

            return Ok(new { message = "Prodotto cancellato con successo." });
        }

        [EnableCors("AnotherPolicy")]
        [HttpPost("CreaUnProdotto")]
        public IActionResult CreaUnProdotto([FromBody] CreaProdottoDto creaProdottoInputDto)
        {
            if (creaProdottoInputDto == null)
            {
                return BadRequest("Input non valido.");
            }

            var prodottoEntity = new CreaProdottoInputDto
            {
                Citta = creaProdottoInputDto.Citta,
                Indirizzo = creaProdottoInputDto.Indirizzo,
                Nome = creaProdottoInputDto.Nome,
                Prezzo = creaProdottoInputDto.Prezzo,
                Provenienza = creaProdottoInputDto.Provenienza
            };

            var prodottoCreato = _prodottoService.CreaProdotto(prodottoEntity);

            if (prodottoCreato == null)
            {
                return BadRequest("Il prodotto non è stato creato.");
            }

            return Ok(prodottoCreato);
        }

        [EnableCors("AnotherPolicy")]
        [HttpPut("ModificaIlNomeDelProdotto/{idProdotto}")]
        public IActionResult ModificaIlNomeDelProdotto(int idProdotto, [FromBody] UpdateProdottoDtoInput modificaIlNomeDelProdottoDtoInput)
        {
            if (modificaIlNomeDelProdottoDtoInput == null)
            {
                return BadRequest("Input non valido.");
            }

            var prodottoModificato = _prodottoService.UpdateProdotto(idProdotto, modificaIlNomeDelProdottoDtoInput);

            if (prodottoModificato == null)
            {
                return NotFound("Prodotto non trovato.");
            }

            _dbContext.SaveChanges();
            return Ok(prodottoModificato);
        }

        [EnableCors("AnotherPolicy")]
        [HttpGet("RestituiscimiUnProdotto/{idProdotto}")]
        public IActionResult RestituiscimiUnProdotto(int idProdotto)
        {
            var prodotto = _prodottoService.GetProdotto(idProdotto);

            if (prodotto == null)
            {
                return NotFound("Prodotto non trovato.");
            }

            return Ok(prodotto);
        }

        [EnableCors("AnotherPolicy")]
        [HttpGet("RestituiscimiLaListaDeiProdotti")]
        public IActionResult RestituiscimiLaListaDeiProdotti()
        {
            var listaProdotti = _prodottoService.GetListProdotti();
            return Ok(listaProdotti);
        }

        [EnableCors("AnotherPolicy")]
        [HttpGet("RestituiscimiLaQuantitàDeiProdotti")]
        public IActionResult RestituiscimiLaQuantitàDeiProdotti()
        {
            var quantitàDeiProdotti = _prodottoService.RestituiscimiLaQuantita();
            return Ok(quantitàDeiProdotti);
        }

        [EnableCors("AnotherPolicy")]
        [HttpGet("NumeroDiElementiCreati")]
        public IActionResult NumeroDiElementiCreati()
        {
            var numeroElementi = _prodottoService.GetNumeroProdottiCreati();
            return Ok(new { numeroElementi });
        }
    }

    
}
