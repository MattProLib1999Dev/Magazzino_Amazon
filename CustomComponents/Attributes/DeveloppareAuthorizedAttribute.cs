using Amazon.AccessTokenComponent.Model;
using Amazon.AccessTokenComponent.Model.Request;
using Amazon.Handlers.Abstratc;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Primitives;

namespace Amazon.CustomComponents.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DeveloppareAuthorizedAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public const string AuhtorizationHeader = "authorization";

        public ILogger<DeveloppareAuthorizedAttribute> logger;

        private string RetrieveAccessToken(AuthorizationFilterContext context, StringValues authorizationHeaderValue)
        {
            if (authorizationHeaderValue.Count > 1)
            {
                context.Result = new BadRequestObjectResult($"{AuhtorizationHeader} invalid format");
                return string.Empty;
            }
            var auhtorizationHeaderSplit = authorizationHeaderValue.First().Split(' ');  
            if (auhtorizationHeaderSplit.Length > 2)
            {
                context.Result = new BadRequestObjectResult($"{AuhtorizationHeader} invalid format");
                return string.Empty;
            }    
            var schema = authorizationHeaderValue[0];

            if (!"bearer".Equals(schema.Trim(' '), StringComparison.InvariantCultureIgnoreCase))
            {
                context.Result = new UnsupportedMediaTypeResult();
                return string.Empty;
            }

            return auhtorizationHeaderSplit[1];
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<DeveloppareAuthorizedAttribute>>();
                IAccessTokenManager developpareAccessToken = context.HttpContext.RequestServices.GetRequiredService<IAccessTokenManager>();

                var existAuthorizationHeader = context.HttpContext.Request.Headers.TryGetValue(AuhtorizationHeader, out StringValues authorizationHeaderValue);

                if (existAuthorizationHeader == false)
                {
                    context.Result = new UnauthorizedObjectResult($"{AuhtorizationHeader} required ");
                    return;
                }

                var AccessToken = RetrieveAccessToken(context, authorizationHeaderValue);
                var result = await developpareAccessToken.Validate(new AccessTokenModelRequest { AccessToken = AccessToken });
                if (result.Status != Common.OperationObjectResultStatus.Ok)
                {
                    context.Result = new UnauthorizedObjectResult($"Access Denied");
                }

                context.HttpContext.Features.Set(result.Value);

                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                context.Result = new UnauthorizedObjectResult($"Access Denied");
                return;
            }
        }
    }
}