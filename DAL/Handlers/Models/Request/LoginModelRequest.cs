using System.ComponentModel.DataAnnotations;
using Amazon.Models.Request;

namespace Amazon.DAL.Handlers.Models.Request
{
	public class LoginModelRequest
	{
	  [Required(ErrorMessage = "Username is required")]
	  [MinLength(4, ErrorMessage = "Must be at least 4 chararacter" )]
	  public string UserName { get; set; } = String.Empty;

	  [Required(ErrorMessage = "Password is required")]		
	  [MinLength(4, ErrorMessage = "Must be at least 4 chararacter" )]
	  public string   Password { get; set; } = String.Empty;

	  public static LoginHandlerRequest From(LoginModelRequest request)
        {
                if (request == null)
                throw new ArgumentNullException(nameof(request), "LoginRequest cannot be null");

                return new LoginHandlerRequest
                {
                Username = request.UserName,
                Password = request.Password
                };
        }
	}
}