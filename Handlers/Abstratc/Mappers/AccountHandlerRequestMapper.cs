using Amazon.DAL.Handlers.Models.Request;
using Amazon.Models.Request;

namespace Amazon.Handlers.Abstratc.Mappers
{
	public static class AccountHandlerRequestMapper
	{
        public static UserInfoDALRequest MapToUserInfoRequest(UserInfoHandlerRequest request)
        {
            return new UserInfoDALRequest
            {
                IdUser = request.IdUser
            };
        }
	}
}