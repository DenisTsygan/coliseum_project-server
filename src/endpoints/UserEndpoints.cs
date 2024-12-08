using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("login", Login);
        app.MapPost("logout", Logout);
        app.MapPost("logoutall", LogoutAllSession);
        app.MapPost("refreshtoken", RefreshToken);
        return app;

    }

    private static async Task<IResult> Login(LoginUserRequest request,
        UserService userService,
        HttpContext context
    )
    {
        var userAgent = context.Request.Headers.UserAgent;
        var ip = context.Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        if (ip.IsNullOrEmpty())
        {
            ip = "ip-is-null";
        }
        var res = await userService.Login(request.Email, request.Password, userAgent, request.FingerPrint, ip);
        context.Response.Cookies.Append("v-appsettings-random-value-refresh-token", res.RefreshToken);
        return Results.Ok(res);
    }

    private static async Task<IResult> RefreshToken(RefreshTokenRequest request,
       UserService userService,
       HttpContext context
   )
    {

        var refreshToken = context.Request.Cookies["v-appsettings-random-value-refresh-token"];
        if (refreshToken.IsNullOrEmpty())
        {
            throw new Exception("Not found refreshToken");
        }

        var userAgent = context.Request.Headers.UserAgent;
        var ip = context.Request.HttpContext.Connection.RemoteIpAddress?.ToString();
        if (ip.IsNullOrEmpty())
        {
            ip = "ip-is-null";
        }
        var res = await userService.RefreshToken(refreshToken, request.FingerPrint, userAgent, ip);
        context.Response.Cookies.Append("v-appsettings-random-value-refresh-token", res.RefreshToken);
        return Results.Ok(res);
    }


    private static async Task<IResult> Logout(
        UserService userService,
        HttpContext context
    )
    {
        var refreshToken = context.Request.Cookies["v-appsettings-random-value-refresh-token"];
        if (refreshToken.IsNullOrEmpty())
        {
            throw new Exception("Not found refreshToken");
        }
        await userService.Logout(refreshToken);
        return Results.Ok("User is logged out from current session.");
    }

    private static async Task<IResult> LogoutAllSession(
        UserService userService,
        HttpContext context
    )
    {
        var refreshToken = context.Request.Cookies["v-appsettings-random-value-refresh-token"];
        if (refreshToken.IsNullOrEmpty())
        {
            throw new Exception("Not found refreshToken");
        }
        await userService.LogoutAllSession(refreshToken);
        return Results.Ok("User is logged out from all session.");
    }

}