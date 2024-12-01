using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public static class ApiExtention
{
    public static void AddMappedEndoints(this IEndpointRouteBuilder app)
    {
        app.MapUsersEndpoints();
        app.MapDataEndpoints();
    }

    public static void AddApiAuthentification(this IServiceCollection services,
            IConfiguration configuration,
            IOptions<JwtOptions> jwtOptions
        )
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["v-appsettings-random-value"];
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();
    }

}