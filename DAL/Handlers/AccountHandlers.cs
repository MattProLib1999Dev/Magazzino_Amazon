using System.Collections.Generic;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request;

public class AccountHandlers : IAccountHandler
{
    private readonly ILogger<AccountHandlers> logger;
    private readonly IAccountDataSource accountDataSource;
    private readonly IAccountHandler accountHandler;

    public AccountHandlers(IAccountDataSource inputAccountDataSource, ILogger<AccountHandlers> inputLogger, IAccountHandler _accountHandler)
    {
        accountDataSource = inputAccountDataSource;
        logger = inputLogger;
        accountHandler = _accountHandler;
    }

    public async Task<List<UserDALResponse>> GetAllUsers()
    {
        try
        {
            var dalResult = await accountDataSource.GetAllUsers();
            var mappedResult = AccountHandlerResponseMapper.MapFromUsersDALResponse(dalResult);

            if (mappedResult.Status == OperationObjectResultStatus.Ok)
            {
                return mappedResult.Value;
            }

            logger.LogError($"Errore durante il recupero degli utenti: {mappedResult.Message}");
            return new List<UserDALResponse>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Errore durante il recupero di tutti gli utenti.");
            return new List<UserDALResponse>();
        }
    }

    public Task<OperationObjectResult<Amazon.Models.Response.LoginHandlerResponse>> Login(LoginHandlerRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<OperationObjectResult<List<UserDALResponse>>> UserInfo(UserInfoHandlerRequest request)
    {
        try
        {
            var mappedRequest = AccountHandlerRequestMapper.MapToUserInfoRequest(request);

            var user = await accountDataSource.UserInfo(mappedRequest);

            if (user.Status != OperationObjectResultStatus.Ok || user.Value == null)
            {
                logger.LogError("Nessun utente trovato o errore nella risposta.");
                return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(
                    OperationObjectResultStatus.NotFound,
                    "No user found."
                );
            }

            var firstUser = user.Value; 

            var userDALResponse = new UserDALResponse
            {
                IdUser = firstUser.IdUser,
                Name = firstUser.Name,
                Surname = firstUser.Surname,
                Username = firstUser.Username
            };

            return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(
                new List<UserDALResponse> { userDALResponse }
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Errore durante l'elaborazione di UserInfo.");

            return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(
                OperationObjectResultStatus.Error,
                "An unexpected error occurred while retrieving user information."
            );
        }
}

    
}



