
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOpions;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _jwtOpions = options.Value;
    }
    public string GenerateToken(UserEntity user)
    {
        Console.WriteLine($"GenerateToken User id {user.Id}");
        Claim[] claims = [
            new(CustomClaims.UserId, user.Id.ToString())
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
}