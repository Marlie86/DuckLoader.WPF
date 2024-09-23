using DuckLoader.WPF.Commands;

using Duckpond.WPF.Common.BaseClasses;
using Duckpond.WPF.Common.Services;
using Duckpond.WPF.Common.Utilities;

using MediatR;

using Duckpond.WPF.Common.Services;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using DuckLoader.WPF.Views;
using DuckLoader.WPF.Utilities;
using AngleSharp;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace DuckLoader.WPF.ViewModels;
/// <summary>
/// Represents the view model for the main window.
/// </summary>
public class MainWindowViewModel : BaseViewModel
{
    #region Private Fields

    private readonly IMediator mediator;

    private readonly NavigationService navigationService;

    private readonly SessionContext sessionContext;
    private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
    private object _CurrentPage;

    private ICommand _SearchVideos;

    private ICommand _WindowClose;

    private ICommand _WindowMaximize;

    private ICommand _WindowMinimize;

    private string videoSearchTerm = string.Empty;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    /// <param name="navigationService">The navigation service instance.</param>
    /// <param name="sessionContext">The session context instance.</param>
    public MainWindowViewModel(IMediator mediator, NavigationService navigationService, SessionContext sessionContext, Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        this.mediator = mediator;
        this.navigationService = navigationService;
        this.sessionContext = sessionContext;
        this.configuration = configuration;
        navigationService.OnNavigateToHandler += OnNavigateToHandler;
        if (configuration.GetValue<bool>("FirstTime") == true)
        {
            CurrentPage = AppStatics.ServiceProvider.GetRequiredService<Options>();
        }
        //C
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// Gets or sets the current page.
    /// </summary>
    public object CurrentPage
    {
        get { return _CurrentPage; }
        set { _CurrentPage = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets the command to search for videos.
    /// </summary>
    public ICommand SearchVideos
    {
        get
        {
            if (_SearchVideos == null)
            {
                _SearchVideos = new RelayCommand<object>(
                    async (p) =>
                    {
                        sessionContext.VideoSearch = VideoSearchTerm;
                        navigationService.NavigateTo(AppStatics.ServiceProvider.GetRequiredService<VideoList>());
                    });
            }
            return _SearchVideos;
        }
    }

    public ICommand NavigateToOptions
    {
        get
        {
            return new RelayCommand<object>(
                async (p) =>
                {
                    navigationService.NavigateTo(AppStatics.ServiceProvider.GetRequiredService<Options>());
                });
        }
    }

    /// <summary>
    /// Gets or sets the search term for videos.
    /// </summary>
    public string VideoSearchTerm
    {
        get { return videoSearchTerm; }
        set { videoSearchTerm = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets the command to close the window.
    /// </summary>
    public ICommand WindowClose
    {
        get
        {
            if (_WindowClose == null)
            {
                _WindowClose = new RelayCommand<object>(
                    async (p) =>
                    {
                        Application.Current.Shutdown();
                    });
            }
            return _WindowClose;
        }
    }
    /// <summary>
    /// Gets the command to maximize or restore the window.
    /// </summary>
    public ICommand WindowMaximize
    {
        get
        {
            if (_WindowMaximize == null)
            {
                _WindowMaximize = new RelayCommand<object>(
                    async (p) =>
                    {
                        if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                        {
                            Application.Current.MainWindow.WindowState = WindowState.Maximized;
                        }
                        else
                        {
                            Application.Current.MainWindow.WindowState = WindowState.Normal;
                        }
                    });
            }
            return _WindowMaximize;
        }
    }

    /// <summary>
    /// Gets the command to minimize the window.
    /// </summary>
    public ICommand WindowMinimize
    {
        get
        {
            if (_WindowMinimize == null)
            {
                _WindowMinimize = new RelayCommand<object>(
                    async (p) =>
                    {
                        Application.Current.MainWindow.WindowState = WindowState.Minimized;
                    });
            }
            return _WindowMinimize;
        }
    }

    #endregion Public Properties

    #region Private Methods

    private void OnNavigateToHandler(object sender, object page)
    {
        CurrentPage = page;
    }

    #endregion Private Methods
}
