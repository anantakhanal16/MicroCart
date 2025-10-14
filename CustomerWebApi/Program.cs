using CustomerWebApi.Extensions;
using CustomerWebApi.Interface;
using CustomerWebApi.Services;
using JwtAuthenticationManager.ServiceExtensions;
using Messaging.Interfaces;
using Messaging.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthExtension();
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddRabbitMq();
builder.Services.AddTransient<ICustomerService, CustomerService>();


builder.Services.AddOpenApi();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

builder.Services.AddSwaggerDocs();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
