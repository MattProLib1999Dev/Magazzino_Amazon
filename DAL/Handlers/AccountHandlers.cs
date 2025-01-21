using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.DoubleOptInComponent.Abastract;
using Amazon.Handlers.Abstratc;
using Amazon.Handlers.Abstratc.Mappers;
using Amazon.Models.Request;
using Amazon.Models.Response;

public class AccountHandlers : IAccountHandler
{
    private readonly ILogger<AccountHandlers> _logger;
    private readonly IAccountDataSource _accountDataSource;
    private readonly IAccessTokenManager _accessTokenManager;
    private readonly IDoubleOptInManager _doubleOptInManager;
    private object _passwordHasher;

    public List<UserDALResponse> Users { get; } = new List<UserDALResponse>();
    public int IdAutoincrememntPrimaryKeyUser { get; private set; }

    public AccountHandlers(
        IAccountDataSource accountDataSource,
        ILogger<AccountHandlers> logger,
        IAccessTokenManager accessTokenManager,
        IDoubleOptInManager doubleOptInManager)
    {
        _accountDataSource = accountDataSource ?? throw new ArgumentNullException(nameof(accountDataSource));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accessTokenManager = accessTokenManager ?? throw new ArgumentNullException(nameof(accessTokenManager));
        _doubleOptInManager = doubleOptInManager ?? throw new ArgumentNullException(nameof(doubleOptInManager));
    }

    public async Task<OperationObjectResult<List<UserDALResponse>>> GetAllUsers()
    {
        try
        {
            var mappedRequest = _accountDataSource.GetAllUsers();

            var user = await _accountDataSource.GetAllUsers();

            if (user.Status != OperationObjectResultStatus.Ok || user.Value == null)
            {
                _logger.LogError("No user found or error in response.");
                return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(
                    OperationObjectResultStatus.NotFound,
                    "No all user found."
                );
            }

            var firstUser = user.Value;
            var userDALResponse = new UserDALResponse();

            for (int i = 0; i < 0; i++)
            {
                var singleUser = userDALResponse;

                singleUser.IdUser = firstUser[i].IdUser;
                singleUser.Name = firstUser[i].Name;
                singleUser.Surname = firstUser[i].Surname;
                singleUser.Username = firstUser[i].Username;
            }



            return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(
                new List<UserDALResponse> { userDALResponse }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing UserInfo.");

            return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(
                OperationObjectResultStatus.Error,
                "An unexpected error occurred while retrieving user information."
            );
        }


    }

    public async Task<OperationObjectResult<List<UserDALResponse>>> UserInfo(UserInfoHandlerRequest request)
    {
        try
        {
            var dalResult = await _accountDataSource.UserInfo(request);

            if (dalResult.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(dalResult.Status, dalResult.Message);
            }

            // Converte il singolo oggetto in una lista
            var usersList = new List<UserDALResponse> { dalResult.Value };

            return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(usersList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            var users = new List<UserDALResponse>();
            return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(users);
        }
    }

    public async Task<OperationObjectResult<LoginHandlerResponse>> Login(LoginHandlerRequest request)
    {
        var handlerResponse = new LoginHandlerResponse();
        try
        {
            var mappedRequest = Amazon.DAL.Handlers.Models.Request.AccountHandlerRequestMapper.MapToLoginRequest(request);
            var user = await _accountDataSource.Login(mappedRequest);

            if (user.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(user.Status, user.Message);
            }

            var userHandlerResponse = AccountHandlerResponseMapper.MapToUserDALResponse(user);
            var requestAccessToken = AccessTokenModelMapper.MapToAccessTokenModel(userHandlerResponse);

            if (requestAccessToken?.Value == null)
            {
                return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(
                    OperationObjectResultStatus.Error, "Failed to map AccessTokenModel.");
            }

            var result = await _accessTokenManager.GenerateToken(requestAccessToken.Value);

            if (result.Status != OperationObjectResultStatus.Ok || result.Value == null)
            {
                return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(
                    OperationObjectResultStatus.Error, "Failed to generate access token.");
            }

            foreach (var response in mappedRequest)
            {
                 new LoginHandlerResponse      
                {
                    AccessToken = response.AccessToken,
                    IdUser = response.IdUser
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login.");
            return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Error, ex.Message);
        }
        return OperationObjectResult<LoginHandlerResponse>.CreateCorrectResponseGeneric(handlerResponse);
    }

    public async Task<OperationObjectResult<CreateUserHandlerResponse>> CreateUser(OperationObjectResult<List<UserDALResponse>> request)
    {
        try
        {
            var mappedRequest = Amazon.DAL.Handlers.Models.Request.AccountHandlerRequestMapper.MapToCreateUserHandlerRequestList(request.Value);

            var result = await _accountDataSource.CreateUser(mappedRequest);

            var doubleOptInRequest = DoubleOptInRequestMapper.MapToDoubleInModel(result);

            if (doubleOptInRequest?.Value == null || doubleOptInRequest.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<CreateUserHandlerResponse>.CreateErrorResponse(doubleOptInRequest.Status);
            }

            // Extract the first item from the list
            var doubleOptInModel = doubleOptInRequest.Value;
            if (doubleOptInModel == null)
            {
                return OperationObjectResult<CreateUserHandlerResponse>.CreateErrorResponse(
                    OperationObjectResultStatus.Error, "No Double Opt-In model found.");
            }

            var doubleOptInTokenResult = await _doubleOptInManager.GenerateDoubleOptInToken(doubleOptInModel);

            if (doubleOptInTokenResult?.Value == null)
            {
                return OperationObjectResult<CreateUserHandlerResponse>.CreateErrorResponse(
                    OperationObjectResultStatus.Error, "Failed to generate Double Opt-In token.");
            }

            var firstResponse = doubleOptInTokenResult.Value;
            if (firstResponse == null)
            {
                return OperationObjectResult<CreateUserHandlerResponse>.CreateErrorResponse(
                    OperationObjectResultStatus.Error, "No Double Opt-In model response found.");
            }

            var response = new CreateUserHandlerResponse
            {
                IdUser = firstResponse.IdUser,
                Username = firstResponse.Username
            };

            return OperationObjectResult<CreateUserHandlerResponse>.CreateCorrectResponseGeneric(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user creation process.");
            return OperationObjectResult<CreateUserHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Error, ex.Message);
        }
    }



    public async Task<OperationObjectResult<ConfirmUserHandlerResponse>> ConfirmUser(ConfirmUserHandlerRequest request)
    {
        try
        {
            // Map the confirmation request
            var mappedRequest = DoubleOptInRequestMapper.MatToDoubleOptInRequest(request);

            // Verify the double opt-in token
            var verifyResult = await _doubleOptInManager.VerifyDoubleOptInToken(mappedRequest);

            // Check if verification was successful
            if (verifyResult.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<ConfirmUserHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.BadRequest);
            }

            // Map the verification result to a list of UserDALResponse
            var dalRequest = verifyResult.Value.Select(optIn => new ConfirmUserHandlerRequest
            {
                IdUser = optIn.IdUser,
                Username = optIn.Username,
                // Map other properties here as necessary
            }).ToList();


            // Call to confirm the user
            var result = await _accountDataSource.ConfirmUser(dalRequest);

            // Map the confirmed user response
            return AccountHandlerResponseMapper.MapUserResponseConfirmUser(result);
        }
        catch (Exception ex)
        {
            // Log errors and return error response with the message
            _logger.LogError(ex, "Error during user confirmation process");
            return OperationObjectResult<ConfirmUserHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Ok, ex.Message);
        }
    }


    public Task<OperationObjectResult<UserDALResponse>> CreateUser(CreateUserDALRequest request)
    {
        try
        {
            var user = Users.FirstOrDefault(u => u.isValid() && u.Username.Equals(request.Username, StringComparison.InvariantCultureIgnoreCase));
            if (user == null)
            {
                var response = new UserDALResponse
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Username = request.Username,
                    Password = request.Password,
                    Status = UserStatus.Created
                };

                return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateCorrectResponseGeneric(response));
            }
            else if (user.Status == UserStatus.Created)
            {
                user.AccountSecuritySalt = Guid.NewGuid().ToString("N");
                return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.Conflict, "User Already Exists"));
            }

            // Handle other statuses or unexpected cases
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.BadRequest, "Bad request User Status"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.Error));
        }
    }
}