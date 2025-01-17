using System.ComponentModel.DataAnnotations;

namespace Amazon.Models
{
	public class CreateUserModelRequest
	{
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





	}
}