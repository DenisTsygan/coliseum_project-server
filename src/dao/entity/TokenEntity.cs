public class Tokenntity
{
    public int Id { get; set; }
    public UserEntity? User { get; set; }//fingerPrint ?

    public Guid UserId { get; set; }

    public string DateSignIn { get; set; } = string.Empty;

    public string DateTokenUpdate { get; set; } = string.Empty;

    public string ValidRefreshToken { get; set; } = string.Empty;

}