public interface IPermissionService
{
    Task<HashSet<Permission>> GetPermissionAsync(Guid userId);

}