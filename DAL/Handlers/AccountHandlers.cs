using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.DoubleOptInComponent.Abastract;
using Amazon.Handlers.Abstract;
using Amazon.Handlers.Abstratc;
using Amazon.Handlers.Abstratc.Mappers;
using Amazon.Models.Request;
using Amazon.Models.Response;

public class AccountHandlers : IAccountHandler
{
    private readonly ILogger<AccountHandlers> logger;
    private readonly IAccountDataSource accountDataSource;

    public readonly IAccessTokenManager accessTokenManager;
    private readonly IAccountDataSource _accountDataSource;
    private readonly IDoubleOptInManager _doubleOptInManager;
    private readonly ILogger<AccountHandler> _logger;

    public List<UserDALResponse> Users = new List<UserDALResponse>();

    public AccountHandlers(IAccountDataSource inputAccountDataSource, ILogger<AccountHandlers> inputLogger, IAccessTokenManager _accessTokenManager, IAccountDataSource accountDataSource, IDoubleOptInManager doubleOptInManager)
    {
        accountDataSource = inputAccountDataSource;
        logger = inputLogger;
        accessTokenManager = _accessTokenManager;
        accountDataSource = _accountDataSource;
        _doubleOptInManager = doubleOptInManager;
    }

    public async Task<OperationObjectResult<List<UserDALResponse>>> GetAllUsers()
    {
        try
        {
            var mappedRequest = accountDataSource.GetAllUsers();

            var user = await accountDataSource.GetAllUsers();

            if (user.Status != OperationObjectResultStatus.Ok || user.Value == null)
            {
                logger.LogError("No user found or error in response.");
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
            logger.LogError(ex, "Error processing UserInfo.");

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
        var dalResult = await accountDataSource.UserInfo(request);

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
        logger.LogError(ex.Message);

        var users = new List<UserDALResponse>();
        return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseSingleObj(users, "Operation succeeded");
      }
    }

    public async Task<OperationObjectResult<List<LoginHandlerResponse>>> Login(LoginHandlerRequest request)
{
    try
    {
        // Mappa la richiesta di login al formato del sistema
        var mappedRequest = Amazon.DAL.Handlers.Models.Request.AccountHandlerRequestMapper.MapToLoginRequest(request);

        // Esegui il login con l'accesso al data source
        var user = await accountDataSource.Login(mappedRequest);

        // Verifica lo stato della risposta
        if (user.Status != OperationObjectResultStatus.Ok)
        {
            return OperationObjectResult<List<LoginHandlerResponse>>.CreateErrorResponse(user.Status, user.Message);
        }

        // Mappa la risposta utente per il token di accesso
        var userHandlerResponse = AccountHandlerResponseMapper.MapFromUserResponseForAccessToken(user);

        // Mappa il modello di accesso del token
        var requestAccessToken = AccessTokenModelMapper.MapToAccessTokenModel(userHandlerResponse);

        // Genera il token di accesso
        // Seleziona il primo elemento dalla lista per passarlo al metodo
        var result = await accessTokenManager.GenerateToken(requestAccessToken.Value.First());


        // Verifica lo stato della generazione del token
        if (result.Status != OperationObjectResultStatus.Ok)
        {
            return OperationObjectResult<List<LoginHandlerResponse>>.CreateErrorResponse(result.Status, result.Message);
        }

        // Controlla la presenza di token generati
        if (result.Value == null || !result.Value.Any())
        {
            return OperationObjectResult<List<LoginHandlerResponse>>.CreateErrorResponse(OperationObjectResultStatus.Error, "No access tokens were generated.");
        }

        // Crea la lista di risposte basata sui token generati
        var responseList = result.Value.Select(token => new LoginHandlerResponse
        {
            AccessToken = token.Accesstoken, // Assumendo che `Accesstoken` sia corretto
            IdUser = token.IdUser
        }).ToList();

        // Restituisci la risposta corretta
        return OperationObjectResult<List<LoginHandlerResponse>>.CreateCorrectResponseGeneric(responseList);
    }
    catch (Exception ex)
    {
        // Log dell'errore e restituzione di una risposta di errore
        logger.LogError(ex, "An error occurred during the Login operation.");
        return OperationObjectResult<List<LoginHandlerResponse>>.CreateErrorResponse(OperationObjectResultStatus.Error, ex.Message);
    }
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

                // Estrarre il primo elemento dalla lista
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
    Task<OperationObjectResult<LoginHandlerResponse>> IAccountHandler.Login(LoginHandlerRequest request)
    {
        throw new NotImplementedException();
    }

    Task<OperationObjectResult<List<UserDALResponse>>> IAccountHandler.CreateUser(OperationObjectResult<List<UserDALResponse>> request)
    {
        throw new NotImplementedException();
    }

    public Task<OperationObjectResult<ConfirmUserHandlerResponse>> ConfirmUser(ConfirmUserHandlerRequest request)
    {
        throw new NotImplementedException();
    }
}