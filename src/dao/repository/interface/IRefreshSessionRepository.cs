public interface IRefreshSessionRepository
{
    Task Add(RefreshSessionEntity refreshSessionEntity);

    Task<RefreshSessionEntity> GetByRefreshToken(string refreshToken);

    Task Delete(RefreshSessionEntity refreshSessionEntity);

    Task DeleteAllByUserId(Guid userId);

    Task<List<RefreshSessionEntity>> GetList();
}