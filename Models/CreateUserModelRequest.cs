using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Amazon.Models.Request;

namespace Amazon.Models
{
	public class CreateUserModelRequest
	{

        [Key , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Must be at least 4 chararcters")]
        public string Name { get; set; } = String.Empty;

        [Required]
        [MinLength(4, ErrorMessage = "Must be at least 4 chararcters")]
        public string Surname { get; set; } = String.Empty;

        [Required]
        [MinLength(4, ErrorMessage = "Must be at least 4 chararcters")]
        public string Username { get; set; } = String.Empty;

        [Required]
        [MinLength(4, ErrorMessage = "Must be at least 4 chararcters")]
        public string Password { get; set; } = String.Empty;

        [Required]
        [MinLength(4, ErrorMessage = "Must be at least 4 chararcters")]
        public string ConfirmPassword { get; set; } = String.Empty;

        public static LoginHandlerRequest From(CreateUserModelRequest request)
        {
                if (request == null)
                throw new ArgumentNullException(nameof(request), "LoginRequest cannot be null");

                return new LoginHandlerRequest
                {
                Username = request.Username,
                Password = request.Password
                };
        }





	}
}