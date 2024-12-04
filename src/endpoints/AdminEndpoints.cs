using Microsoft.AspNetCore.Mvc;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("admin-page", AdminPage);//TODO show html page
        app.MapGet("sessions", GetListSessions).RequirePermissions(Permission.WATCH_SESSIONS);
        return app;

    }

    private static async Task<IResult> AdminPage()
    {
        var html = "<h1>LOLOLOLOLO ADMIN Page</h1>";
        return Results.Ok(html);
    }

    private static async Task<IResult> GetListSessions(
        RefreshSessionService refreshSessionService
    )
    {
        var res = await refreshSessionService.GetListSessions();
        return Results.Ok(res);
    }
}