using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddScoped<UserService>();
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IUserRepository, UserRepository>();

// Получение настроек JwtOptions
var jwtOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>();
services.AddApiAuthentification(configuration, jwtOptions);

var app = builder.Build();


//var options = configuration.GetSection(nameof(JwtOptions));
//ApiExtention.AddApiAuthentification(services,configuration , options);
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    //Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();
app.AddMappedEndoints();

//TODO add exeption midleware
//app.UseMiddleware<ExeptionMidleware>()

app.MapGet("/test", () => "123123213213");

UserRepository.
        users.Add(User.Create(Guid.NewGuid(), "hahahname", "123pppfdg", "email"));//User.Create(Guid.NewGuid(), "hahahname", "123pppfdg", "email")
//System.Console.WriteLine("123123213hfdshfhdshfhsd");
/*app.Run(async (context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    var stringBuilder = new System.Text.StringBuilder("<table>");

    foreach (var header in context.Request.Headers)
    {
        stringBuilder.Append($"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>");
    }
    stringBuilder.Append("</table>");
    await context.Response.WriteAsync(stringBuilder.ToString());
});*/
app.Run();
