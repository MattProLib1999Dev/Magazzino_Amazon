namespace Amazon.DAL.Models.Response
{
    public class UserDALResponse
    {
        public long IdUser { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string ErrorMessage { get; set; } = String.Empty;
        public string ErrorCode { get; set; } = String.Empty;
        public string OriginalPassword { get; set; } = String.Empty;
        public string PasswordSecuritySalt { get; set; } = String.Empty;
    }

}