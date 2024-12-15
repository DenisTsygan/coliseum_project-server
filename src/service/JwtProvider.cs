
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOpions;

    private readonly IRefreshSessionRepository _refreshSessionRepository;

    public JwtProvider(IOptions<JwtOptions> options, IRefreshSessionRepository refreshSessionRepository)
    {
        _jwtOpions = options.Value;
        _refreshSessionRepository = refreshSessionRepository;
    }

    public string GenerateAccessToken(UserEntity user)
    {
        string roles = "";
        if (user.Roles is not null)
        {
            roles = JsonSerializer.Serialize(user.Roles);//TODO serialize first letter variable on lover case and fix in coliseum_project_frontend->UserInfo->IRole->IPermission
        }
        Claim[] claims = [
            new(CustomClaims.UserId, user.Id.ToString()),
            new(CustomClaims.UserName, user.UserName.ToString()),
            new(CustomClaims.Email, user.Email.ToString()),
            new(CustomClaims.Roles, roles)
        ];
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOpions.SecretKey)
            ), SecurityAlgorithms.HmacSha256
        );
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddSeconds(_jwtOpions.ExpireAccessToken)
        );
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    public async Task<string> GenerateRefreshToken(Guid userId, string userAgent, string fingerprint, string ip)
    {

        //TODO add service
        var refreshSessinId = Guid.NewGuid();
        var refreshToken = Guid.NewGuid();
        var expiresIn = DateTime.UtcNow.AddSeconds(_jwtOpions.ExpireRefreshToken);

        var refreshSession = new RefreshSessionEntity
        {
            Id = refreshSessinId,
            UserId = userId,
            RefreshToken = refreshToken,
            UserAgent = userAgent,
            FingerPrint = fingerprint,
            Ip = ip,
            ExpiresIn = expiresIn
        };
        await _refreshSessionRepository.Add(refreshSession);

        Claim[] claims = [
            new(CustomClaims.UserId, userId.ToString()),
            new(CustomClaims.RefreshSessionId, refreshToken.ToString()),
        ];
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOpions.SecretKey)
            ), SecurityAlgorithms.HmacSha256
        );
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: expiresIn
        );
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    public string GetClaimFromToken(string token, string claimName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        // Проверяем, валиден ли токен и поддерживается ли формат
        if (!tokenHandler.CanReadToken(token))
            throw new ArgumentException("Invalid token format");

        // Пытаемся извлечь токен как JwtSecurityToken
        var jwtToken = tokenHandler.ReadJwtToken(token);

        // Ищем нужный Claim по имени
        var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimName);

        // Если Claim не найден, выбрасываем исключение или возвращаем null
        if (claim == null)
            throw new Exception($"Claim '{claimName}' not found in the token");

        return claim.Value;
    }
}