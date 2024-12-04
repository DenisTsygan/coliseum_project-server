using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

public static class ApiExtention
{
    public static void AddMappedEndoints(this IEndpointRouteBuilder app)
    {
        app.MapUsersEndpoints();
        app.MapDataEndpoints();
        app.MapAdminEndpoints();
    }

    public static void AddApiAuthentification(
        this IServiceCollection services,
            IConfiguration configuration
        )
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        services
            .AddAuthentication(optins =>
            {
                optins.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                optins.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                optins.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey)),
                    ClockSkew = TimeSpan.Zero//По умолчанию, в ASP.NET Core добавляется временное смещение (ClockSkew) для обработки возможных рассинхронизаций между сервером и клиентом. По умолчанию оно составляет 5
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Token = authHeader["Bearer ".Length..].Trim();
                            Console.WriteLine("context.Token = " + authHeader["Bearer ".Length..].Trim());
                        }
                        //context.Token = context.Request.Cookies["v-appsettings-random-value"];
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddAuthorization();
    }

    public static IEndpointConventionBuilder RequirePermissions<TBuilder>(
        this TBuilder builder, params Permission[] permissions
    ) where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireAuthorization(policy => policy.AddRequirements(
            new PermissionRequirement(permissions)
        ));
    }

}