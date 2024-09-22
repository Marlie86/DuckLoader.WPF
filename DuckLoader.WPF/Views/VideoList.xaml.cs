using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DuckLoader.WPF.Views;

/// <summary>
/// Represents a UserControl for displaying a list of videos.
/// </summary>
public partial class VideoList : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VideoList"/> class.
    /// </summary>
    public VideoList()
    {
        InitializeComponent();
    }

    private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if(e.VerticalOffset % 5 == 0)
        {
            var vieModel = this.DataContext as ViewModels.VideoListViewModel;
            //vieModel?.SearchVideos();
        }
    }
}
