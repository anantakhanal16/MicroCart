using OrderApi.Application.Interface;
using OrderApi.Infrastructure.Extensions;
using OrderApi.Infrastructure.Services;
using OrderApi.Infrastructure.Services.GrpcServices;
using OrderApi.Infrastructure.Services.GrpcServices.ServerSideStreaming;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(82); // ?? match docker expectations
//});
builder.Services.AddGrpc();

builder.Services.AddMongoDb((builder.Configuration));
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHostedService<RabbitMqConsumerService>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(82, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        // You can add UseHttps() if you want HTTPS
        // listenOptions.UseHttps();
    });
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

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<PaymentGrpcService>();
app.MapGrpcService<GrpcStreamDemoService>();

app.Run();
