public class JwtOptions
{

    public string SecretKey { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public int ExpireAccessToken { get; set; } = 0;
    public int ExpireRefreshToken { get; set; } = 0;
}