using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{

    private readonly ServiceDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserRepository(ServiceDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<User> Add(UserEntity user, int roleId)
    {
        var roleEntiy = await _dbContext.Roles
            .SingleOrDefaultAsync(r => r.Id == roleId)//TODO change role (int)Role.Accountant
            ?? throw new Exception("InvalidOperationExeption");

        var userEntity = new UserEntity()
        {
            Id = user.Id,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            Email = user.Email,
            Roles = [roleEntiy]
        };

        await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
        var userEntityFromDb = await _dbContext.Users
            .AsNoTracking()
             .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Id == user.Id);
        return _mapper.Map<User>(userEntityFromDb);
    }
    public async Task<UserEntity> GetByEmail(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception("Not found user");
    }
    public async Task<UserEntity> GetById(Guid id)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception("Not found user");
    }

    public async Task<HashSet<Permission>> GetUserPermissions(Guid userId)
    {
        var roles = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => (Permission)p.Id)
            .ToHashSet();
    }

    public async Task<List<User>> GetList()
    {
        var userEntities = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .ToListAsync();

        return _mapper.Map<List<User>>(userEntities);
    }


    //Additional 
    public async Task<List<UserEntity>> GetByFilter(string email, string userName)
    {

        var query = _dbContext.Users.AsNoTracking();
        if (string.IsNullOrEmpty(email))
        {
            query = query.Where(u => u.Email.Contains(email));
        }
        if (string.IsNullOrEmpty(userName))
        {
            query = query.Where(u => u.UserName.Contains(userName));
        }

        return await query.ToListAsync();
    }

    //pageSize - 10 єлементов на странице ; page - 5 страница
    public async Task<List<UserEntity>> GetByPage(int page, int pageSize)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Dictionary<Guid, User>> GetListByIds(HashSet<Guid> guids)
    {
        var userEntities = await _dbContext.Users
        .AsNoTracking()
        .Include(u => u.Roles)
        .ThenInclude(r => r.Permissions)
        .Where(u => guids.Contains(u.Id))
        .ToDictionaryAsync(u => u.Id);

        return _mapper.Map<Dictionary<Guid, User>>(userEntities);
    }
    public async Task DeleteById(Guid userId)
    {
        await _dbContext.Users.Where(u => u.Id == userId).ExecuteDeleteAsync();
    }
}