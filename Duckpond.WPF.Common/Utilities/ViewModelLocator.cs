using Duckpond.WPF.Common.BaseClasses;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Duckpond.WPF.Common.Utilities;

/// <summary>
/// Provides a dynamic view model locator for resolving and retrieving view models.
/// </summary>
public class ViewModelLocator : DynamicObject
{
    private readonly IServiceProvider serviceProvider = AppStatics.ServiceProvider;

    private Dictionary<string, Type> ViewModels = new Dictionary<string, Type>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelLocator"/> class.
    /// </summary>
    public ViewModelLocator()
    {
        LoadViewModels();
    }

    /// <summary>
    /// Tries to get the specified view model by name.
    /// </summary>
    /// <param name="binder">The binder representing the member being accessed.</param>
    /// <param name="result">The result of the member access.</param>
    /// <returns><c>true</c> if the view model was found; otherwise, <c>false</c>.</returns>
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        var viewModel = GetViewModel(binder.Name);
        if (viewModel != null)
        {
            result = viewModel;
            return true;
        }
        result = new object();
        return false;
    }

    /// <summary>
    /// Gets the view model with the specified name.
    /// </summary>
    /// <param name="viewModelName">The name of the view model.</param>
    /// <returns>The view model instance, or <c>null</c> if not found.</returns>
    public object? GetViewModel(string viewModelName)
    {
        if (ViewModels.TryGetValue(viewModelName, out Type? viewModelType))
        {
            return serviceProvider.GetRequiredService(viewModelType);
        }
        return null;
    }

    /// <summary>
    /// Loads the view models into the dictionary.
    /// </summary>
    private void LoadViewModels()
    {
        AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t =>
            {
                return t.IsClass && t.BaseType != null && t.BaseType == typeof(BaseViewModel);
            })
            .ToList()
            .ForEach(t =>
            {
                var viewModelName = t.Name;
                var viewModelType = t;
                ViewModels.Add(viewModelName, viewModelType);
            });
    }
}
