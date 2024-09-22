using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duckpond.WPF.Common.Utilities;
/// <summary>
/// Provides static properties and methods for accessing application services and configuration.
/// </summary>
public static class AppStatics
{
    /// <summary>
    /// Gets or sets the service provider.
    /// </summary>
    public static IServiceProvider ServiceProvider { get; set; }

    /// <summary>
    /// Gets or sets the configuration.
    /// </summary>
    public static IConfiguration Configuration { get; set; }

    /// <summary>
    /// Gets the view model locator.
    /// </summary>
    public static ViewModelLocator ViewModelLocator => ServiceProvider.GetRequiredService<ViewModelLocator>();
}
