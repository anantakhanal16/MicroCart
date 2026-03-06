using CustomerWebApi.Extensions;
using CustomerWebApi.Grpc;
using CustomerWebApi.GrpcClient;
using CustomerWebApi.Interface;
using CustomerWebApi.Services;
using CustomerWebApi.Services.Grpc;
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
builder.Services.AddScoped<PaymentService>();

builder.Services.AddOpenApi();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
    options.ListenAnyIP(5005, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});
builder.Services.AddGrpcClient<PaymentGrpc.PaymentGrpcClient>(options =>
{
    options.Address = new Uri("http://orderapi:82"); 
});
builder.Services.AddGrpc();

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
app.MapGrpcService<PaymentGrpcService>();

app.MapControllers();

app.Run();
