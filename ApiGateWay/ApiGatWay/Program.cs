using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load configuration before adding services and building the app
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
       .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // ?? important
});
// Add Ocelot to the DI container before building the app
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Use Ocelot middleware
await app.UseOcelot();

app.Run();
