using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;
public class Installer : IExtensionsInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddConfigurations(config.GetSection(nameof(DataSetOptions)), new DataSetOptions());
    }
}

public static class ServiceExtensions
{
    public static void AddConfigurations<TConfig>(this IServiceCollection services, IConfigurationSection section, TConfig config)
    where TConfig : class, new()
    {
        section.Bind(config);
        services.AddSingleton(config);
    }
}
