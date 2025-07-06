namespace human_resource_management.Service
{
    public class TokenCleanupJob
    {
        private readonly JwtService _jwtService;

        public TokenCleanupJob(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task Execute()
        {
            await _jwtService.CleanUpExpiredRefreshTokens();
        }
    }
}
