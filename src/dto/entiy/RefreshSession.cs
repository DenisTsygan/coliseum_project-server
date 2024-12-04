public class RefreshSession
{
    public Guid Id { get; set; }
    public User User { get; set; }

    public Guid RefreshToken { get; set; }
    public string UserAgent { get; set; } = string.Empty;

    public string FingerPrint { get; set; } = string.Empty;

    public string Ip { get; set; } = string.Empty;

    public DateTime ExpiresIn { get; set; }
}