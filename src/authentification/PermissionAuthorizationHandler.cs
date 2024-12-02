using Microsoft.AspNetCore.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        Console.WriteLine("HandleRequirementAsync start ");

        var userId = context.User.Claims.FirstOrDefault(
            c => c.Type == CustomClaims.UserId
        );

        if (userId is null || !Guid.TryParse(userId.Value, out var id))
        {
            Console.WriteLine("HandleRequirementAsync return ");
            return; // not found user
        }
        using var scope = _serviceScopeFactory.CreateScope();
        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
        var permissions = await permissionService.GetPermissionAsync(id);

        if (permissions.ToList().Intersect(requirement.Permissions).Any())
        {
            context.Succeed(requirement);
        }
    }
}