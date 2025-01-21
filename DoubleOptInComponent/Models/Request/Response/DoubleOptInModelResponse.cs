namespace Amazon.DoubleOptInComponent.Models.Request.Response
{
	public class DoubleOptInModelResponse
	{
        public string DoubleOptInToken { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
		public long IdUser { get; set; }
        public string AccesstokenModel { get; set; } = String.Empty;
		public string AccountSecuritySalt {get; set; } = String.Empty;
		public string Name {get; set; } = String.Empty;
		public string Surname {get; set; } = String.Empty;
		public string Password {get; set; } = String.Empty;
		


    }
}