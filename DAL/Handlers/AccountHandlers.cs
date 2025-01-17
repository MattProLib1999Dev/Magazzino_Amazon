using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.Handlers.Abstratc;
using Amazon.Models.Request;
using Amazon.Models.Response;

public class AccountHandlers : IAccountHandler
{
    private readonly ILogger<AccountHandlers> logger;
    private readonly IAccountDataSource accountDataSource;

    public readonly IAccessTokenManager accessTokenManager;

    public List<UserDALResponse> Users = new List<UserDALResponse>();

    public AccountHandlers(IAccountDataSource inputAccountDataSource, ILogger<AccountHandlers> inputLogger, IAccessTokenManager _accessTokenManager)
    {
        accountDataSource = inputAccountDataSource;
        logger = inputLogger;
        accessTokenManager = _accessTokenManager;
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

    public async Task<OperationObjectResult<LoginHandlerResponse>> Login(LoginHandlerRequest request)
    {
        try
        {
            // Mappa la richiesta di login al formato del sistema
            var mappedRequest = AccountHandlerRequestMapper.MapToLoginRequest(request);

            // Esegui il login con l'accesso al data source
            var user = await accountDataSource.Login(mappedRequest);

            // Se lo stato della risposta non è OK, restituisci un errore
            if (user.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(user.Status, user.Message);
            }

            // Mappa la risposta utente per il token di accesso
            var userHandlerResponse = AccountHandlerResponseMapper.MapFromUserResponseForAccessToken(user);

            // Mappa il modello di accesso del token
            var requestAccessToken = AccessTokenModelMapper.MapToAccessTokenModel(userHandlerResponse);

            // Genera il token di accesso
            var result = await accessTokenManager.GenerateToken(requestAccessToken.Value);

            // Se la generazione del token non è andata a buon fine, restituisci un errore
            if (result.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(result.Status, result.Message);
            }

            // Verifica che ci siano elementi nella lista
            if (result.Value == null || !result.Value.Any())
            {
                return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Error, "No access tokens were generated.");
            }

            // Accedi al primo elemento della lista
            var firstToken = result.Value.First();

            // Mappa il risultato del token crittografato a una risposta da restituire
            
                var responseList = new LoginHandlerResponse
                {
                    AccessToken = firstToken.Accesstoken, // Assumendo che `Accesstoken` sia una proprietà valida in AccessTokenEncriptModel
                    IdUser = firstToken.IdUser
                };

            // Restituisci una risposta corretta con la lista di LoginHandlerResponse
            return OperationObjectResult<LoginHandlerResponse>.CreateCorrectResponseGeneric(responseList);
        }
        catch (Exception ex)
        {
            // Log dell'errore e restituzione di una risposta di errore
            logger.LogError(ex.Message);
            return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Error, ex.Message);
        }
    }

    public async Task<OperationObjectResult<CreateUserHandlerResponse>> CreateUser(CreateUserHandlerRequest request)
    {
         try
            {
              var mappedRequest = AccountHandlerRequestMapper.MapToCreateUserRequest(request);
              var result = await accountDataSource.CreateUser(mappedRequest);
              return AccountHandlerResponseMapper.MapFromCreateUsersHandlerResponse(result);
            }
            catch (Exception ex)
            {
              logger.LogError(ex.Message);
              return OperationObjectResult<CreateUserHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Error, ex.Message);
            }
    }
}