public static class DataEndpoints
{
    public static IEndpointRouteBuilder MapDataEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("api");
        endpoints.MapGet("data", GetData).RequireAuthorization();
        endpoints.MapGet("kil", () => "kakafka 1231 23");
        endpoints.MapGet("secure", () => "hahahahahahh its secured)))").RequireAuthorization();
        return app;

    }

    private static async Task<IResult> GetData()
    {
        await Task.Delay(1);
        //await authUserService.Register(request.UserName, request.Email, request.Password);
        return Results.Ok("confidntional data");
    }

}