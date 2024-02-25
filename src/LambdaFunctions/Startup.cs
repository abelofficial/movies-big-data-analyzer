using System.IO;
using LambdaFunctions.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LambdaFunctions;

public class Startup
{

    public static IConfigurationRoot BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    public static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        var configuration = BuildConfiguration();

        services.InstallServicesFromAssembly(configuration);
        return services;
    }
}


