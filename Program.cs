using AutoMapper;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

var configuration = builder.Configuration;
var services = builder.Services;

var connectionString = configuration.GetConnectionString("AppDbConnectionString");
services.AddDbContext<ServiceDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))//"8.0.20"
);

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

services.AddAutoMapper(typeof(UserMapperProfile));

services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddScoped<UserService>();
services.AddScoped<RefreshSessionService>();
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IRefreshSessionRepository, RefreshSessionRepository>();

services.AddApiAuthentification(configuration);

var app = builder.Build();


//var options = configuration.GetSection(nameof(JwtOptions));
//ApiExtention.AddApiAuthentification(services,configuration , options);
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();
app.AddMappedEndoints();
app.UseCors();
app.UseStaticFiles();

// Настройка маршрута для SPA-приложения React
app.MapWhen(context => context.Request.Path.StartsWithSegments("/admin"), adminApp =>
{
    adminApp.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin")),
        RequestPath = "/admin"
    });

    adminApp.Run(async context =>
    {
        // Если путь не ведет к файлу, возвращаем index.html
        context.Response.ContentType = "text/html";
        await context.Response.SendFileAsync(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin", "index.html"));
    });
});


//TODO entity Framework core - workwith bd
//TODO add exeption midleware
//app.UseMiddleware<ExeptionMidleware>()

app.MapGet("/test", () => "123123213213");

//UserRepository.
//users.Add(User.Create(Guid.NewGuid(), "hahahname", "123pppfdg", "email"));//User.Create(Guid.NewGuid(), "hahahname", "123pppfdg", "email")
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
