using Amazon.AccessTokenComponent.Model;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.CustomComponents.Attributes;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.Models.Request;
using Amazon.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Amazon;

[Route("[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountHandler _accountHandler;

    public AccountController(ILogger<AccountController> logger, IAccountHandler accountHandler)
    {
        _logger = logger;
        _accountHandler = accountHandler;
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var dalResult = await _accountHandler.GetAllUsers();
            return Ok(dalResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero di tutti gli utenti.");
            return StatusCode(500, new ErrorResponse
            {
                ErrorMessage = "An unexpected error occurred while retrieving users.",
                ErrorCode = "INTERNAL_SERVER_ERROR"
            });
        }
    }

    [HttpGet("UserInfo")]
    public async Task<IActionResult> UserInfo([FromQuery] UserInfoHandlerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorMessage = "Invalid request data.",
                ErrorCode = "VALIDATION_ERROR"
            });
        }

        try
        {
            var mappedRequest = AccountRequestMapper.MapToUserInfoRequest(request);
            var userHandler = await _accountHandler.UserInfo(mappedRequest);
            var user = AccountHandlerResponseMapper.MapFromUsersDALResponse(userHandler);

            if (user == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorMessage = "User not found.",
                    ErrorCode = "NOT_FOUND"
                });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in {MethodName}", nameof(UserInfo));
            return StatusCode(500, new ErrorResponse
            {
                ErrorMessage = "An unexpected error occurred while retrieving user information.",
                ErrorCode = "INTERNAL_SERVER_ERROR"
            });
        }
    }




[HttpPost("Login")]
public async Task<IActionResult> Login([FromBody] LoginHandlerRequest request)
{
    try
    {
        // Validazione della richiesta
        if (request == null)
        {
            _logger.LogWarning("Richiesta di login ricevuta è null.");
            return BadRequest("La richiesta è null.");
        }

        // Mappatura della richiesta
        var mappedRequest = AccountRequestMapper.MapToLoginRequest(request);
        if (mappedRequest == null)
        {
            _logger.LogWarning("Errore durante la mappatura della richiesta di login.");
            return BadRequest("Errore nella mappatura della richiesta.");
        }

        // Login tramite il gestore account
        var loginResponse = await _accountHandler.Login(mappedRequest);
        if (loginResponse == null)
        {
            _logger.LogError("Il gestore account ha restituito una risposta null.");
            return StatusCode(500, "Errore nel gestore account.");
        }

        // Mappatura della risposta
        var response = AccountResponseMapper.MapFromLoginHandlerResponse(loginResponse);
        if (response == null)
        {
            _logger.LogError("Errore durante la mappatura della risposta del gestore account.");
            return StatusCode(500, "Errore interno durante la mappatura della risposta.");
        }

        // Controllo dello stato della risposta
        return HandleResponseStatus(response);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Errore imprevisto durante l'operazione di login.");
        return StatusCode(500, "Errore interno del server.");
    }
}

// Metodo helper per gestire lo stato della risposta
private IActionResult HandleResponseStatus(OperationObjectResult<UserInfoModelResponse> response)
{
    if (response.Status == OperationObjectResultStatus.Ok)
    {
        _logger.LogInformation("Login effettuato con successo per l'utente.");
        return Ok(response.Value);
    }

    _logger.LogWarning($"Stato imprevisto della risposta: {response.Status}. Messaggio: {response.Message}");
    return StatusCode((int)response.Status, response.Message ?? "Stato non previsto.");
}




    [HttpGet("Test")]
    [DeveloppareAuthorized]
    public IActionResult TestAuth()
    {
        var feature = HttpContext.Features.Get<AccessTokenModel>();
        return Ok(feature);
    }






    

    public class ErrorResponse
    {
        public string ErrorMessage { get; set; } = String.Empty;
        public string ErrorCode { get; set; } = String.Empty;
    }
}