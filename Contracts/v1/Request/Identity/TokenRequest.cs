namespace CommunityAPI.Contracts.v1.Request.Identity
{
    public class TokenRequest
    {
        public string UserId { get; set; }

        public string RefreshToken { get; set; }
    }
}
