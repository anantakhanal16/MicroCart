using OrderApi.Extensions;
using OrderApi.Interface;
using OrderApi.Services;
using ProductApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // ?? match docker expectations
});
builder.Services.AddMongoDb((builder.Configuration));
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddSwaggerDocs();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
