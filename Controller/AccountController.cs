using Amazon.Appunti.Handlers.Abstract;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response.Mappers;
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
            var user = AccountHandlerResponseMapper.MapFromUserInfoHandlerResponse(userHandler);

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
}
public class ErrorResponse
{
    public string ErrorMessage { get; set; } = String.Empty;
    public string ErrorCode { get; set; } = String.Empty;
}
