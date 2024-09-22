using AngleSharp;

using AutoMapper;

using DuckLoader.WPF.Utilities;
using DuckLoader.WPF.ViewModels;
using DuckLoader.WPF.Views;

using Duckpond.WPF.Common.Utilities;
using Duckpond.WPF.Common.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Navigation;

using Configuration = System.Configuration.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace DuckLoader.WPF;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Event handler for the application startup.
    /// </summary>
    /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        AppStatics.Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IConfiguration>(AppStatics.Configuration);
        ConfigureServices(serviceCollection);
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        AppStatics.ServiceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = AppStatics.ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    /// <summary>
    /// Configures the services for dependency injection.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    private void ConfigureServices(ServiceCollection serviceCollection)
    {
        var mapper = new MapperConfiguration(cfg =>
        {
        }).CreateMapper();
        serviceCollection.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });
        serviceCollection.AddSingleton(mapper);
        serviceCollection.AddSingleton<Duckpond.WPF.Common.Services.NavigationService>();
        serviceCollection.AddSingleton<SessionContext>();

        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient<MainWindowViewModel>();
        serviceCollection.AddTransient<VideoList>();
        serviceCollection.AddTransient<VideoListViewModel>();
    }
}

