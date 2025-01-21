using System.ComponentModel.DataAnnotations;

namespace Amazon.Models.Request
{
	public class ConfirmUserModelRequest
	{
        [Required]
        public string DoubleOptInToken { get; set; } = String.Empty;
	}
}