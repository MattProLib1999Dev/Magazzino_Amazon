using Amazon.AccessTokenComponent.Model;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.CustomComponents.Attributes;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.Handlers.Abstract;
using Amazon.Models;
using Amazon.Models.Request;
using Amazon.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Amazon;

[Route("[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountHandler _accountHandler;

    private readonly IMediator _mediator;

    public AccountController(ILogger<AccountController> logger, IAccountHandler accountHandler, IMediator mediator)
    {
        _logger = logger;
        _accountHandler = accountHandler;
        _mediator = mediator;
    }

    [HttpGet("GetAllUsers")]
    public async Task<UserDALResponse> GetAllUsers()
    {
         var user = await _mediator.Send(new UserDALResponse());
        return user;
    }

    [HttpGet("GetUser")]
    public async Task<UserDALResponse?> GetUser(int idUser)
    {
        var user = await _mediator.Send(new UserDALResponse() { IdUser = idUser});
        return user;
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

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginModelRequest request)
{
    if (request == null || string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
    {
        _logger.LogWarning("Invalid login request: missing username or password.");
        return BadRequest("Invalid request. Username and password are required.");
    }

    try
    {
        var handlerRequest = LoginModelRequest.From(request);
        var result = await _accountHandler.Login(handlerRequest);

        if (result.Status != OperationObjectResultStatus.Ok)
        {
            _logger.LogWarning("Login failed: {Status}, Message: {Message}", result.Status, result.Message);
            return StatusCode((int)result.Status, result.Message);
        }

        _logger.LogInformation("User logged in successfully: {Username}", request.UserName);
        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An unexpected error occurred during login for Username: {Username}", request.UserName);
        return Problem(detail: "An error occurred during login. Please try again later.", statusCode: StatusCodes.Status500InternalServerError);
    }
}

// Metodo helper per gestire lo stato della risposta
private IActionResult HandleResponseStatus(OperationObjectResult<CreateUserHandlerResponse> response)
{
    if (response == null)
    {
        _logger.LogError("La risposta è null.");
        return StatusCode(500, "Errore interno del server: la risposta non può essere null.");
    }

    if (response.Status == OperationObjectResultStatus.Ok)
    {
        _logger.LogInformation("Utente creato con successo.");
        return Ok(response.Value); // Restituisce il valore direttamente
    }

    _logger.LogWarning($"Errore durante la creazione dell'utente: {response.Status}. Messaggio: {response.Message}");
    return StatusCode((int)response.Status, response.Message ?? "Errore sconosciuto durante la creazione dell'utente.");
}

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


    [HttpPost("Confirm")]
    public async Task<UserDALResponse> CreateUser([FromBody] int userId)
    {
        var customerDetails = await _mediator.Send(new UserDALResponse() { IdUser = userId});
        return customerDetails;
    }
    public class ErrorResponse
    {
        public string ErrorMessage { get; set; } = String.Empty;
        public string ErrorCode { get; set; } = String.Empty;
    }
}