using System.ComponentModel.DataAnnotations;

namespace Amazon.DAL.Handlers.Models.Request
{
	public class UserInfoHandlerRequest
	{
	    [Required]
        [Range(1, long.MaxValue)]
        public long IdUser { get; set; }
	}
}