using Duckpond.WPF.Common.BaseClasses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DuckLoader.WPF.Models;
/// <summary>
/// Represents a video search result.
/// </summary>
public class VideoSearchResultModel : BaseNotifyPropertyChanged
{
    private Visibility isDownloading = Visibility.Collapsed;
    private Visibility isDownloaded = Visibility.Collapsed;

    /// <summary>
    /// Gets or sets the title of the video.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the author of the video.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL of the video.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the duration of the video.
    /// </summary>
    public string Duration { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the thumbnail URL of the video.
    /// </summary>
    public string ThumbnailUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the visibility of the downloading indicator.
    /// </summary>
    public Visibility IsDownloading { get => isDownloading; set { isDownloading = value; OnPropertyChanged(); } }

    /// <summary>
    /// Gets or sets the visibility of the downloaded indicator.
    /// </summary>
    public Visibility IsDownloaded { get => isDownloaded; set { isDownloaded = value; OnPropertyChanged(); } }
}
