using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Routing.Patterns;

namespace Amazon.DAL.Handlers.Models.Request
{
	public class CreateUserHandlerRequest
	{
        [Required(ErrorMessage = "Username is required")]
        [MinLength(4, ErrorMessage = "Must be at least 4 chararacter" )]
        public string UserName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Password is required")]		
        [MinLength(4, ErrorMessage = "Must be at least 4 chararacter" )]
        public string   Name { get; set; } = String.Empty;
        [Required(ErrorMessage = "Password is required")]		
        [MinLength(4, ErrorMessage = "Must be at least 4 chararacter" )]
        public string   Surname { get; set; } = String.Empty;
        [Required(ErrorMessage = "Password is required")]		
        [MinLength(4, ErrorMessage = "Must be at least 4 chararacter" )]
        public string   Password { get; set; } = String.Empty;


	}
}