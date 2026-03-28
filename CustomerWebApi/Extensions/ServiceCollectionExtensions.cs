using CustomerWebApi.Grpc;
using CustomerWebApi.GrpcClient;
using CustomerWebApi.Interface;
using CustomerWebApi.Services;
using JwtAuthenticationManager.ServiceExtensions;
using StreamDemo.Grpc;


namespace CustomerWebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddCustomJwtAuthExtension();
        services.AddAppDbContext(configuration);
        services.AddRabbitMq();

        services.AddTransient<ICustomerService, CustomerService>();
        services.AddScoped<IGrpcStremClient, GrpcStremClient>(); 

        services.AddScoped<PaymentService>();

        services.AddGrpc();

        var grpcBaseAddress = new Uri("http://orderapi:82");

        services.AddGrpcClient<PaymentGrpc.PaymentGrpcClient>(o => o.Address = grpcBaseAddress);
        services.AddGrpcClient<StreamDemoGrpc.StreamDemoGrpcClient>(o => o.Address = grpcBaseAddress);
        services.AddSwaggerDocs();
        services.AddOpenApi();

        return services;
    }
}