using Duckpond.WPF.Common.BaseClasses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DuckLoader.WPF.Models;
public class VideoSearchResultModel: BaseNotifyPropertyChanged
{
    private Visibility isDownloading = Visibility.Collapsed;

    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public Visibility IsDownloading { get => isDownloading; set { isDownloading = value; OnPropertyChanged(); } }
}
