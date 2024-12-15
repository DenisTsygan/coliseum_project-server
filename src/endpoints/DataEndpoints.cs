public static class DataEndpoints
{
    public static IEndpointRouteBuilder MapDataEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("api/data");

        endpoints.MapGet("data", GetData).RequirePermissions(Permission.WATCH_DATA);
        //endpoints.MapGet("datalol", GetData).RequirePermissions(Permission.SEND_NOTIFICATION);
        //endpoints.MapGet("datalolkek", GetData).RequirePermissions(Permission.ADD_ACCOUNTANT);

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