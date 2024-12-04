public interface IJwtProvider
{
    string GenerateAccessToken(UserEntity user);

    Task<string> GenerateRefreshToken(Guid userId, string userAgent, string fingerprint, string ip);

    string GetClaimFromToken(string token, string claimName);

}