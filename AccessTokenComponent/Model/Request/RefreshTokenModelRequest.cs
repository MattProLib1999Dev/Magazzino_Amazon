namespace Amazon.AccessTokenComponent.Model.Request
{
	public class RefreshTokenModelRequest
	{
        public string AccessToken { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
	}
}