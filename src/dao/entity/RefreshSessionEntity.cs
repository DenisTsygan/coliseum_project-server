public class RefreshSessionEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public Guid RefreshToken { get; set; }//refresh token id (id = refresh token)
    public string UserAgent { get; set; } = string.Empty;

    public string FingerPrint { get; set; } = string.Empty;

    public string Ip { get; set; } = string.Empty;

    public DateTime ExpiresIn { get; set; }
}