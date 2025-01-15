using Amazon.AccessTokenComponent.Model;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request;
using Amazon.Models.Response;

namespace Amazon.Handlers.Abstratc
{
  public class AccountHandler : IAccountHandler
  {
    public readonly ILogger<AccountHandler> logger;
    public readonly IAccountDataSource accountDataSource;
    public readonly IAccessTokenManager accessTokenManager;
    public AccountHandler(IAccountDataSource inputAccountDataSource, ILogger<AccountHandler> inputLogger, IAccessTokenManager inputAccessTokenManager)
    {
      accountDataSource = inputAccountDataSource;
      logger = inputLogger;
      accessTokenManager = inputAccessTokenManager;
    }

    public async Task<OperationObjectResult<List<UserDALResponse>>> GetAllUsers()
    {
      try
      {
        var dalResult = await accountDataSource.GetAllUsers();
        var resultMapped = AccountHandlerResponseMapper.MapFromUsersDALResponse(dalResult);
        return resultMapped;
      }
      catch (Exception ex)
      {

        logger.LogError(ex.Message);
        var users = new List<UserDALResponse>();

        return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseSingleObj(users, "Operation succeeded");
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
        var mappedRequest = AccountHandlerRequestMapper.MapToLoginRequest(request);

        // Esegui il login con l'accesso al data source
        var user = await accountDataSource.Login(mappedRequest);

        // Se lo stato della risposta non è OK, restituisci un errore
        if (user.Status != OperationObjectResultStatus.Ok)
        {
            return OperationObjectResult<List<LoginHandlerResponse>>.CreateErrorResponse(user.Status, user.Message);
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
            return OperationObjectResult<List<LoginHandlerResponse>>.CreateErrorResponse(result.Status, result.Message);
        }

        // Mappa il risultato del token crittografato a una risposta da restituire
        var responseList = new List<LoginHandlerResponse>
        {
            new LoginHandlerResponse
            {
                AccessToken = result.Value.Accesstoken,
                IdUser = result.Value.IdUser
            }
        };

        // Restituisci una risposta corretta con la lista di LoginHandlerResponse
        return OperationObjectResult<List<LoginHandlerResponse>>.CreateCorrectResponseGeneric(responseList);
    }
    catch (Exception ex)
    {
        // Log dell'errore e restituzione di una risposta di errore
        logger.LogError(ex.Message);
        return OperationObjectResult<List<LoginHandlerResponse>>.CreateErrorResponse(OperationObjectResultStatus.Error, ex.Message);
    }
}

  }
}
