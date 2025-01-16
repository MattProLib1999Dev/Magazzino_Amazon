using System.ComponentModel.DataAnnotations;

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
	}
}