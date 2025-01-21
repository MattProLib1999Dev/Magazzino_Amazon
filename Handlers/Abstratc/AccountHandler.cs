using Amazon.AccessTokenComponent.Model.Abstract;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.DoubleOptInComponent.Abastract;
using Amazon.DoubleOptInComponent.Models;
using Amazon.DoubleOptInComponent.Models.Request.Response;
using Amazon.Handlers.Abstratc.Mappers;
using Amazon.Models.Request;
using Amazon.Models.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountHandlerRequestMapper = Amazon.DAL.Handlers.Models.Request.AccountHandlerRequestMapper;

namespace Amazon.Handlers.Abstract
{
    public class AccountHandler : IAccountHandler
    {
        private readonly ILogger<AccountHandler> _logger;
        private readonly IAccountDataSource _accountDataSource;
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IDoubleOptInManager _doubleOptInManager;

        public AccountHandler(
            IAccountDataSource accountDataSource,
            ILogger<AccountHandler> logger,
            IAccessTokenManager accessTokenManager,
            IDoubleOptInManager doubleOptInManager)
        {
            _accountDataSource = accountDataSource;
            _logger = logger;
            _accessTokenManager = accessTokenManager;
            _doubleOptInManager = doubleOptInManager;
        }

        public async Task<OperationObjectResult<List<UserDALResponse>>> GetAllUsers()
        {
            try
            {
                var dalResult = await _accountDataSource.GetAllUsers();
                var resultMapped = AccountHandlerResponseMapper.MapFromUsersDALResponse(dalResult);
                return resultMapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users.");
                return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseSingleObj(
                    new List<UserDALResponse>(), "Operation succeeded with no results.");
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

                var usersList = new List<UserDALResponse> { dalResult.Value };
                return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(usersList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user info.");
                return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseSingleObj(
                    new List<UserDALResponse>(), "Operation succeeded with no results.");
            }
        }

        public async Task<OperationObjectResult<LoginHandlerResponse>> Login(LoginHandlerRequest request)
        {
            try
            {
                var mappedRequest = DAL.Handlers.Models.Request.AccountHandlerRequestMapper.MapToLoginRequest(request);
                var user = await _accountDataSource.Login(mappedRequest);

                if (user.Status != OperationObjectResultStatus.Ok)
                {
                    return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(user.Status, user.Message);
                }

                var userHandlerResponse = AccountHandlerResponseMapper.MapToUserDALResponse(user);
                var requestAccessToken = AccessTokenModelMapper.MapToAccessTokenModel(userHandlerResponse);
                var result = await _accessTokenManager.GenerateToken(requestAccessToken.Value);

                if (result.Status != OperationObjectResultStatus.Ok || result.Value == null)
                {
                    return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(
                        OperationObjectResultStatus.Error, "Failed to generate access token.");
                }

                var firstToken = result.Value;

                var response = new LoginHandlerResponse
                {
                    AccessToken = firstToken.Accesstoken,
                    IdUser = firstToken.IdUser
                };

                return OperationObjectResult<LoginHandlerResponse>.CreateCorrectResponseGeneric(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login.");
                return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Error, ex.Message);
            }
        }

       public async Task<OperationObjectResult<CreateUserHandlerResponse>> CreateUser(OperationObjectResult<List<UserDALResponse>> request)
{
    try
    {
        // Mappiamo la richiesta
        var mappedRequest = AccountHandlerRequestMapper.MapFromUsersDALResponse(request);

        // Creiamo l'utente
        var result = await _accountDataSource.CreateUser(mappedRequest);

        var mappedResult = AccountHandlerResponseMapper.MapFromUsersDALResponse(result);

        // Mappiamo la risposta dal datasource
        var mappedResponse = AccountHandlerRequestMapper.MapFromUsersDALResponse(result);

        // Gestione del Double Opt-In
        var doubleOptInRequest = DoubleOptInRequestMapper.MapToDoubleInModel(mappedResponse);

        // Se la risposta del Double Opt-In non è Ok, restituiamo un errore
        if (doubleOptInRequest.Status != OperationObjectResultStatus.Ok)
        {
            return OperationObjectResult<CreateUserHandlerResponse>.CreateErrorResponse(doubleOptInRequest.Status);
        }

        // Avvolgi doubleOptInRequest.Value in un OperationObjectResult
        var doubleOptInModelResult = new OperationObjectResult<DoubleOptInModel>
        {
            Status = OperationObjectResultStatus.Ok,
            Value = doubleOptInRequest.Value,
            Message = "Success"
        };

        // Se la generazione del token è avvenuta con successo, continua con la logica
        var doubleOptInTokenResult = await _doubleOptInManager.GenerateDoubleOptInToken(doubleOptInModelResult);

        // Verifica lo stato della risposta
        if (doubleOptInTokenResult.Status != OperationObjectResultStatus.Ok || doubleOptInTokenResult.Value == null || !doubleOptInTokenResult.Value.Any())
        {
            return OperationObjectResult<CreateUserHandlerResponse>.CreateErrorResponse(
                OperationObjectResultStatus.Error, "Failed to generate Double Opt-In token.");
        }

        // Se la lista non è vuota, estrai il primo oggetto dalla lista
        var firstDoubleOptInModelResponse = doubleOptInTokenResult.Value.FirstOrDefault();

        // Verifica se esiste almeno un oggetto nella lista
        if (firstDoubleOptInModelResponse == null)
        {
            return OperationObjectResult<CreateUserHandlerResponse>.CreateErrorResponse(
                OperationObjectResultStatus.Error, "No Double Opt-In model response found.");
        }

        // Mappiamo il modello DoubleOptInModelResponse in un oggetto UserDALResponse
        var userDALResponse = new UserDALResponse
        {
            IdUser = firstDoubleOptInModelResponse.IdUser,
            Name = firstDoubleOptInModelResponse.Name,
            Surname = firstDoubleOptInModelResponse.Surname,
            Username = firstDoubleOptInModelResponse.Username,
            Password = firstDoubleOptInModelResponse.Password,
            AccountSecuritySalt = firstDoubleOptInModelResponse.AccountSecuritySalt,
            AccesstokenModel = firstDoubleOptInModelResponse.AccesstokenModel
        };

        // Logica per la risposta finale
        var response = new CreateUserHandlerResponse
        {
            IdUser = userDALResponse.IdUser,
            Username = userDALResponse.Username
        };

        // Restituiamo la risposta con il risultato
        return OperationObjectResult<CreateUserHandlerResponse>.CreateCorrectResponseGeneric(response);
    }
    catch (Exception ex)
    {
        // Gestione dell'errore con log dettagliato
        _logger.LogError(ex, "Error during user creation process");

        // Restituiamo un errore
        return OperationObjectResult<CreateUserHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Error, ex.Message);
    }
}

      
    }
}
