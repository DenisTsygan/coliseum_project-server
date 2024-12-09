using Microsoft.AspNetCore.Mvc;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("api");
        endpoints.MapGet("sessions", GetListSessions).RequirePermissions(Permission.WATCH_SESSIONS);
        endpoints.MapPost("logout", LogoutByRSId).RequirePermissions(Permission.WATCH_SESSIONS);
        endpoints.MapGet("users", GetList).RequirePermissions(Permission.ADD_ACCOUNTANT);
        endpoints.MapDelete("users/{userId}", DeleteUserById).RequirePermissions(Permission.ADD_ACCOUNTANT);//TODO permissions on every request?
        endpoints.MapPost("register", Register);
        return app;

    }

    private static async Task<IResult> GetListSessions(
        RefreshSessionService refreshSessionService
    )
    {
        var res = await refreshSessionService.GetListSessions();
        return Results.Ok(res);
    }

    private static async Task<IResult> LogoutByRSId(
        LogoutByRSIdRequest request,
        UserService userService,
        HttpContext context
    )
    {
        await userService.LogoutByRefres(request.Rsid);
        return Results.Ok("User is logged out from current session.");
    }

    private static async Task<IResult> GetList(
        UserService userService
    )
    {
        var res = await userService.GetList();
        return Results.Ok(res);
    }

    private static async Task<IResult> DeleteUserById(
       string userId,
       UserService userService,
       HttpContext context
   )
    {
        var isGuid = Guid.TryParse(userId, out var uid);
        if (isGuid)
        {
            await userService.LogoutAllSessionByUserId(uid);
            await userService.DeleteByUserId(uid);
        }
        else
        {
            throw new Exception("Param userid is not guid");
        }
        //await userService.LogoutByRefres(request.Rsid);
        return Results.Ok("User and sessions is deleted.");
    }

    private static async Task<IResult> Register(RegisterUserRequest request, UserService authUserService)
    {
        var user = await authUserService.Register(request.UserName, request.Email, request.Password, request.RoleId);
        return Results.Ok(user);
    }
}