namespace Amazon.AccessTokenComponent.Model.Dummy
{
	public class RefreshTokenObject
	{
        public AccessTokenModel AccessTokenModel { get; set; }

        public string AccessToken { get; set; }

        public static RefreshTokenObject Create(string accessToken, AccessTokenModel accessTokenModel)
        {
            return new RefreshTokenObject { AccessToken = accessToken, AccessTokenModel = accessTokenModel};
        }
	}
}