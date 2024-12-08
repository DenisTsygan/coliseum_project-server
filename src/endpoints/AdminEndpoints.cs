using Microsoft.AspNetCore.Mvc;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("api");
        endpoints.MapGet("sessions", GetListSessions).RequirePermissions(Permission.WATCH_SESSIONS);
        endpoints.MapPost("logout", LogoutByRSId).RequirePermissions(Permission.WATCH_SESSIONS);
        endpoints.MapGet("users", GetList).RequirePermissions(Permission.ADD_ACCOUNTANT);
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

    private static async Task<IResult> Register(RegisterUserRequest request, UserService authUserService)
    {
        var user = await authUserService.Register(request.UserName, request.Email, request.Password, request.RoleId);
        return Results.Ok(user);
    }
}