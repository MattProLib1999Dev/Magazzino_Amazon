using Amazon.AccessTokenComponent.Model.Abstract;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.DoubleOptInComponent.Abastract;
using Amazon.DoubleOptInComponent.Models;
using Amazon.Handlers.Abstratc.Mappers;
using Amazon.Models.Request;
using Amazon.Models.Response;
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
                var mappedRequest = AccountHandlerRequestMapper.MapToLoginRequest(request);
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

                var response = new LoginHandlerResponse
                {
                    AccessToken = result.Value.Accesstoken,
                    IdUser = result.Value.IdUser
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
                var mappedRequest = AccountHandlerRequestMapper.MapToCreateUserHandlerRequestList(request.Value);

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

        Task<OperationObjectResult<List<UserDALResponse>>> IAccountHandler.CreateUser(OperationObjectResult<List<UserDALResponse>> request)
        {
            throw new NotImplementedException();
        }
    }
}
