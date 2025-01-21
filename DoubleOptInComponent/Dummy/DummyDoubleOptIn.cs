using Amazon.Common;
using Amazon.DoubleOptInComponent.Abastract;
using Amazon.DoubleOptInComponent.Models;
using Amazon.DoubleOptInComponent.Models.Request;
using Amazon.DoubleOptInComponent.Models.Request.Response;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Caching.Memory;

namespace Amazon.DoubleOptInComponent.Dummy
{
    public class DummyDoubleOptIn : IDoubleOptInManager
    {

        private const string DoubleOptInConstant = "DoubleOptIn_";
        private readonly IMemoryCache memoryCache;
        private const int TimeCache = 1;

        public DummyDoubleOptIn(IMemoryCache inputMemoryCache)
        {
            memoryCache = inputMemoryCache;
        }

        public Task<OperationObjectResult<DoubleOptInModelResponse>> GenerateDoubleOptInToken(DoubleOptInModel input)
        {
            if (input == null || input.IdUser < 1 || string.IsNullOrEmpty(input.Username))
                return Task.FromResult(OperationObjectResult<DoubleOptInModelResponse>.CreateErrorResponse(OperationObjectResultStatus.BadRequest));
            var doubleOptInToken_key = $"{DoubleOptInConstant}{input.DoubleOptInToken}";
            memoryCache.Set(doubleOptInToken_key, input, TimeSpan.FromMinutes(TimeCache));
            return Task.FromResult(OperationObjectResult<DoubleOptInModelResponse>.CreateCorrectResponseGeneric(
                new DoubleOptInModelResponse
                {
                    DoubleOptInToken = input.DoubleOptInToken,
                    Username = input.Username
                }
            ));
        }

        public Task<OperationObjectResult<List<DoubleOptInModelResponse>>> VerifyDoubleOptInToken(DoubleOptInModel request)
        {
            var doubleOptInTokenKey = $"{DoubleOptInConstant}{request.DoubleOptInToken}";

            // Verifica se il token esiste nella cache
            if (memoryCache.TryGetValue(doubleOptInTokenKey, out DoubleOptInModel cachedOptIn))
            {
                // Verifica che il token corrisponda
                if (cachedOptIn.DoubleOptInToken.Equals(request.DoubleOptInToken, StringComparison.InvariantCulture))
                {
                    // Rimuove il token dalla cache dopo il successo
                    memoryCache.Remove(doubleOptInTokenKey);

                    // Mappatura di DoubleOptInModel a una lista di DoubleOptInModelResponse
                    var response = new List<DoubleOptInModelResponse>
                    {
                        new DoubleOptInModelResponse
                        {
                            // Mappatura dei dati da DoubleOptInModel a DoubleOptInModelResponse
                            // Adatta questa mappatura in base alle proprietà specifiche dei tuoi modelli
                            DoubleOptInToken = cachedOptIn.DoubleOptInToken,
                                IdUser = cachedOptIn.IdUser,
                            Username = cachedOptIn.Username,
                            Password = cachedOptIn.Password,
                        
                        }
                    };

                    // Restituisce una risposta corretta con la lista
                    return Task.FromResult(OperationObjectResult<List<DoubleOptInModelResponse>>.CreateCorrectResponseGeneric(response));
                }
            }

            // Restituisce una risposta di errore nel caso in cui il token non sia trovato o non corrisponda
            return Task.FromResult(OperationObjectResult<List<DoubleOptInModelResponse>>.CreateErrorResponse(OperationObjectResultStatus.NotFound));
        }

        public Task<OperationObjectResult<List<DoubleOptInModelResponse>>> VerifyDoubleOptInToken(DoubleOptInModelRequest user)
        {
            throw new NotImplementedException();
        }
    }
}