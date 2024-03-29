using Quizzify.Client.Host;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR().AddHubOptions<ClientHostHub>(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromMinutes(1);
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/", () => "Hello World!");
app.MapHub<ClientHostHub>("/host");

app.Run();