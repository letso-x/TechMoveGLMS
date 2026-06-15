namespace TechMoveGLMS.Services
{
    public class TokenService
    {
        private const string TokenSessionKey = "jwt_token";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void StoreToken(string token)
        {
            _httpContextAccessor.HttpContext?.Session.SetString(TokenSessionKey, token);
        }

        public string? GetToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString(TokenSessionKey);
        }
    }
}
