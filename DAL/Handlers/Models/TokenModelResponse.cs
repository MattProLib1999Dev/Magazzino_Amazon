namespace Amazon.DAL.Handlers.Models
{
	public class TokenModelResponse
	{
        public string AccessToken { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
	}
}