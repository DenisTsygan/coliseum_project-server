
using Microsoft.EntityFrameworkCore;

public class RefreshSessionRepository : IRefreshSessionRepository
{
    private readonly ServiceDbContext _dbContext;
    public RefreshSessionRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(RefreshSessionEntity refreshSessionEntity)//Guid userId, string userAgent, string fingerPrint, string ip, DateTime expiresIn, DateTime createdAt
    {
        await _dbContext.RefreshSessions.AddAsync(refreshSessionEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(RefreshSessionEntity refreshSessionEntity)
    {
        await _dbContext.RefreshSessions.Where(rs => rs.Id == refreshSessionEntity.Id).ExecuteDeleteAsync();
    }

    public async Task DeleteAllByUserId(Guid userId)
    {
        await _dbContext.RefreshSessions.Where(rs => rs.UserId == userId).ExecuteDeleteAsync();
    }

    public async Task<RefreshSessionEntity> GetByRefreshToken(string refreshToken)
    {
        return await _dbContext.RefreshSessions
            .AsNoTracking().FirstOrDefaultAsync(rs => rs.RefreshToken.ToString() == refreshToken) ?? throw new Exception("Not found RefreshSession");
    }

    public async Task<List<RefreshSessionEntity>> GetList()
    {
        return await _dbContext.RefreshSessions.AsNoTracking().ToListAsync();
    }
}