using Amazon.Models.Request;
using Amazon.Models.Response;
using MediatR;

namespace Amazon.DAL.Models.Response
{
    public enum UserStatus { Created, Confirmed, Ok, Error, TemporaryRedirect, NotFound };
    public class UserDALResponse : IRequest<UserDALResponse>
    {
        public long IdUser { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string ErrorMessage { get; set; } = String.Empty;
        public string ErrorCode { get; set; } = String.Empty;
        public string OriginalPassword { get; set; } = String.Empty;
        public string PasswordSecuritySalt { get; set; } = String.Empty;
        public string AccountSecuritySalt { get; set; } = String.Empty;
        public string DoubleOptInToken { get; set; } = String.Empty;
        public string AccesstokenModel { get; set; } = String.Empty;
        public string AccessToken { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
        public CreateUserHandlerResponse User { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; } = String.Empty;

        public UserStatus Status { get; set; }

        public bool isValid()
        {
            return Status == UserStatus.Confirmed;
        }

        public bool NeedConfirm()
        {
            return Status == UserStatus.Created;
        }
    }

}