public interface IJwtProvider
{
    string GenerateToken(UserEntity user);

}