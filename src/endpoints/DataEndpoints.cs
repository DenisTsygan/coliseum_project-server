public static class DataEndpoints
{
    public static IEndpointRouteBuilder MapDataEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("api/data");
        endpoints.MapGet("init", Init);// generate data dev endpoint

        endpoints.MapGet("get/{periodName}", GetData).RequirePermissions(Permission.WATCH_DATA);
        endpoints.MapPost("name", RenameById).RequirePermissions(Permission.WATCH_DATA);

        //endpoints.MapGet("datalol", GetData).RequirePermissions(Permission.SEND_NOTIFICATION);
        //endpoints.MapGet("datalolkek", GetData).RequirePermissions(Permission.ADD_ACCOUNTANT);

        endpoints.MapGet("kil", () => "kakafka 1231 23");
        endpoints.MapGet("secure", () => "hahahahahahh its secured)))").RequireAuthorization();
        return app;

    }

    private static async Task<IResult> GetData(
        string periodName,
        DataService dataService
    )
    {
        var res = await dataService.GetECMByPeriodName(periodName);
        return Results.Ok(res);
    }

    private static async Task<IResult> RenameById(
        RenameClientRequest request,
        DataService dataService
    )
    {
        var isGuid = Guid.TryParse(request.EcmId, out var guid);
        if (!isGuid)
        {
            throw new Exception("Uncorrect guid in request");
        }

        await dataService.RenameClientById(guid, request.NewName);
        return Results.Ok("Success rename client");
    }

    private static async Task<IResult> Init(
       DataService dataService
   )
    {
        await dataService.InitData();
        return Results.Ok("Success");
    }

}