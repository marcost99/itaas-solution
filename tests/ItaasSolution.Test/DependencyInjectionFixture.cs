using ItaasSolution.Api.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

public class DependencyInjectionFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public DependencyInjectionFixture()
    {
        var serviceCollection = new ServiceCollection();

        // Sets the dependencies injections
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        serviceCollection.AddSingleton<IConfiguration>(configuration);

        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddScoped<IInfoFileLog, InfoFileLog>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}
