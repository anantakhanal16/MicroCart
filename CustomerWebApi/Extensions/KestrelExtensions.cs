

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CustomerWebApi.Extensions;

public static class KestrelExtensions
{
    public static void ConfigureCustomKestrel(this ConfigureWebHostBuilder webHost)
    {
        webHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(80);

        });
    }
}