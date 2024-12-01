public interface IJwtProvider
{
    string GenerateToken(User user);

}