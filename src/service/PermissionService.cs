
public class PermissionService : IPermissionService
{

    private readonly IUserRepository _userRepository;
    public PermissionService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task<HashSet<Permission>> GetPermissionAsync(Guid userId)
    {
        return _userRepository.GetUserPermissions(userId);
    }
}