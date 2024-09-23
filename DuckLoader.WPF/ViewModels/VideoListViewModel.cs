using DuckLoader.WPF.Commands;
using DuckLoader.WPF.Models;
using DuckLoader.WPF.Utilities;

using Duckpond.WPF.Common.BaseClasses;
using Duckpond.WPF.Common.Utilities;

using MediatR;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

using YoutubeExplode.Search;

namespace DuckLoader.WPF.ViewModels;
/// <summary>
/// View model for the video list view.
/// </summary>
public class VideoListViewModel : BaseViewModel
{
    #region Private Fields

    private readonly IMediator mediator;
    private readonly IConfiguration configuration;
    private readonly SessionContext sessionContext;
    private bool isLoading = false;
    private ObservableCollection<VideoSearchResultModel> videos = new ObservableCollection<VideoSearchResultModel>();
    private string videoSearchTerm;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoListViewModel"/> class.
    /// </summary>
    /// <param name="sessionContext">The session context.</param>
    /// <param name="mediator">The mediator.</param>
    /// <param name="configuration">The configuration.</param>
    public VideoListViewModel(SessionContext sessionContext, IMediator mediator, IConfiguration configuration)
    {
        this.sessionContext = sessionContext;
        this.mediator = mediator;
        this.configuration = configuration;
        VideoSearchTerm = string.IsNullOrEmpty(sessionContext.VideoSearch) ? "musik" : $"musik {sessionContext.VideoSearch}";
        SearchVideos();
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// Gets the command for downloading a video.
    /// </summary>
    public ICommand DownloadVideoCommand { get { return new RelayCommand<VideoSearchResultModel>(DownloadVideo); } }

    /// <summary>
    /// Gets the command for opening the download directory.
    /// </summary>
    public ICommand OpenDownloadDirectory { get { return new RelayCommand<VideoSearchResultModel>(p => Process.Start("explorer.exe", configuration.GetValue<string>("DownloadDirectory"))); } }

    /// <summary>
    /// Gets or sets a value indicating whether the view model is currently loading.
    /// </summary>
    public bool IsLoading
    {
        get => isLoading; set
        {
            isLoading = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ScrollBoxVisibility));
            OnPropertyChanged(nameof(LoaderVisibility));
        }
    }

    /// <summary>
    /// Gets the visibility of the loader.
    /// </summary>
    public Visibility LoaderVisibility { get => IsLoading ? Visibility.Visible : Visibility.Collapsed; }

    /// <summary>
    /// Gets the visibility of the scroll box.
    /// </summary>
    public Visibility ScrollBoxVisibility { get => !IsLoading ? Visibility.Visible : Visibility.Collapsed; }

    /// <summary>
    /// Gets or sets the collection of video search results.
    /// </summary>
    public ObservableCollection<VideoSearchResultModel> Videos { get => videos; set { videos = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the video search term.
    /// </summary>
    public string VideoSearchTerm { get => videoSearchTerm; set { videoSearchTerm = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the next page token for pagination.
    /// </summary>
    public string NextPage { get; set; } = string.Empty;

    #endregion Public Properties

    #region Private Methods

    /// <summary>
    /// Downloads a video.
    /// </summary>
    /// <param name="item">The video search result model.</param>
    private async void DownloadVideo(VideoSearchResultModel item)
    {
        item.IsDownloading = Visibility.Visible;
        await mediator.Send(new LoadVideoCommand() { VideoUrl = item.Url });
        item.IsDownloading = Visibility.Collapsed;
        item.IsDownloaded = Visibility.Visible;
        OnPropertyChanged(nameof(Videos));
    }

    /// <summary>
    /// Searches for videos.
    /// </summary>
    public async void SearchVideos()
    {
        IsLoading = true;
        if (!NextPage.Equals("end", StringComparison.InvariantCultureIgnoreCase))
        {
            (NextPage, var list) = await mediator.Send(new SearchVideoCommand() { VideoSearchTerm = VideoSearchTerm, PageToken = NextPage });
            var videostoAdd = Enumerable.Range(0, list.Count < 10 ? list.Count : 10).Select(i => list[i]).ToList();
            videostoAdd.ForEach(video =>
            {
                Videos.Add(video);
            });
        }
        IsLoading = false;
    }

    #endregion Private Methods
}
