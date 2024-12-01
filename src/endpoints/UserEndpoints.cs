public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("register", Register);
        app.MapPost("login", Login);
        app.MapGet("hol", () => "kakafka");
        return app;

    }

    private static async Task<IResult> Register(RegisterUserRequest request, UserService authUserService)
    {
        await authUserService.Register(request.UserName, request.Email, request.Password);
        return Results.Ok();
    }
    private static async Task<IResult> Login(LoginUserRequest request,
        UserService userService,
        HttpContext context
    )
    {
        var token = await userService.Login(request.Email, request.Password);
        context.Response.Cookies.Append("v-appsettings-random-value", token);
        return Results.Ok(token);
    }
}