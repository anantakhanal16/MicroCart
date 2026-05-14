
using ProductApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppDbContext(builder.Configuration);

builder.Services.AddControllers();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); 
});
builder.Services.AddOpenApi();
builder.Services.AddSwaggerDocs();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();

app.MapControllers();

app.Run();
