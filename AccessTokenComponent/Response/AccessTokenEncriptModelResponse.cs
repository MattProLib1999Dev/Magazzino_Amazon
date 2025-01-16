namespace Amazon.AccessTokenComponent.Model
{
	public class AccessTokenEncriptModelResponse
	{
		public long IdUser { get; set; }
        public string Accesstoken { get; set; } = String.Empty;
		public string RefeshToken { get; set; } = String.Empty;
		public string Name { get; set; } = String.Empty;
		public string Surname { get; set; } = String.Empty;
		public string Username { get; set; } = String.Empty;
		public string Password { get; set; } = String.Empty;
	}
}