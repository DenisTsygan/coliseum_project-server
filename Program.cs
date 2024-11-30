var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/test", () => "123123213213");

//System.Console.WriteLine("123123213hfdshfhdshfhsd");
app.Run();
