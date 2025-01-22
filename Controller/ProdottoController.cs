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
            _prodottoService = prodottoService;
            _dbContext = dbContext;
            _logger = logger;
        }

        [EnableCors("AnotherPolicy")]
        [HttpDelete("CancellaUnProdotto/{idDelProdotto}")]
        public IActionResult CancellaUnProdotto([FromRoute] int idDelProdotto)
        {
            try
            {
                var prodottoCancellato = _prodottoService.CancellaUnProdotto(idDelProdotto);

                if (prodottoCancellato == null)
                {
                    return NotFound("Prodotto non trovato.");
                }

                return Ok(new { message = "Prodotto cancellato con successo." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la cancellazione del prodotto.");
                return StatusCode(500, "Si è verificato un errore durante la cancellazione del prodotto.");
            }
        }

        [EnableCors("AnotherPolicy")]
        [HttpPost("CreaUnProdotto")]
        public IActionResult CreaUnProdotto([FromBody] CreaProdottoDto creaProdottoInputDto)
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

                var prodottoCreato = _prodottoService.CreaProdotto(prodottoEntity);

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
        public IActionResult ModificaIlNomeDelProdotto(int idProdotto, [FromBody] UpdateProdottoDtoInput modificaIlNomeDelProdottoDtoInput)
        {
            try
            {
                var prodottoModificato = _prodottoService.UpdateProdotto(idProdotto, modificaIlNomeDelProdottoDtoInput);

                if (prodottoModificato == null)
                {
                    return NotFound("Prodotto non trovato.");
                }

                _dbContext.SaveChanges();
                return Ok(prodottoModificato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la modifica del prodotto.");
                return StatusCode(500, "Si è verificato un errore durante la modifica del prodotto.");
            }
        }

        [EnableCors("AnotherPolicy")]
        [HttpGet("RestituiscimiUnProdotto/{idProdotto}")]
        public IActionResult RestituiscimiUnProdotto(int idProdotto)
        {
            try
            {
                var prodotto = _prodottoService.GetProdotto(idProdotto);

                if (prodotto == null)
                {
                    return NotFound("Prodotto non trovato.");
                }

                return Ok(prodotto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero del prodotto.");
                return StatusCode(500, "Si è verificato un errore durante il recupero del prodotto.");
            }
        }

        [EnableCors("AnotherPolicy")]
        [HttpGet("RestituiscimiLaListaDeiProdotti")]
        public IActionResult RestituiscimiLaListaDeiProdotti()
        {
            try
            {
                var listaProdotti = _prodottoService.GetListProdotti();
                return Ok(listaProdotti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero della lista dei prodotti.");
                return StatusCode(500, "Si è verificato un errore durante il recupero della lista dei prodotti.");
            }
        }

        [EnableCors("AnotherPolicy")]
        [HttpGet("RestituiscimiLaQuantitàDeiProdotti")]
        public IActionResult RestituiscimiLaQuantitàDeiProdotti()
        {
            try
            {
                var quantitàDeiProdotti = _prodottoService.RestituiscimiLaQuantita();
                return Ok(quantitàDeiProdotti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero della quantità dei prodotti.");
                return StatusCode(500, "Si è verificato un errore durante il recupero della quantità dei prodotti.");
            }
        }

        [EnableCors("AnotherPolicy")]
        [HttpPut("ModificaLaQuantitàDeiProdotti/{quantita}")]
        public IActionResult ModificaLaQuantitàDeiProdotti([FromRoute] int quantita)
        {
            try
            {
                var quantitàModificata = _prodottoService.ModificaLaQuantitàDeiProdotti(quantita);

                if (quantitàModificata < 0)
                {
                    return BadRequest("Quantità non valida.");
                }

                _dbContext.SaveChanges();
                return Ok(new { message = "Quantità modificata con successo.", quantita = quantitàModificata });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la modifica della quantità dei prodotti.");
                return StatusCode(500, "Si è verificato un errore durante la modifica della quantità dei prodotti.");
            }
        }
    }
}
