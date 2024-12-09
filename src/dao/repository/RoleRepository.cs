
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class RoleRepository : IRoleRepository
{

    private readonly ServiceDbContext _dbContext;
    private readonly IMapper _mapper;

    public RoleRepository(ServiceDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<RoleEntity>> GetList()
    {
        return await _dbContext.Roles
            .AsNoTracking()
            .Include(r => r.Permissions)
            .ToListAsync();
    }
}