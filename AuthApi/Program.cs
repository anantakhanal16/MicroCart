using AuthApi.Extensions;
using AuthApi.Interface;
using JwtAuthenticationManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
