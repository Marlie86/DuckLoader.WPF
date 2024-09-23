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

    /// <summary>
    /// Handles the scroll changed event of the ScrollViewer control.
    /// It triggers the search for more videos when the user scrolls to the near bottom of the list.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="ScrollChangedEventArgs"/> instance containing the event data.</param>
    private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        var scrollViewer = (ScrollViewer)sender;
        if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight * 0.9)
        {
            var viewModel = (ViewModels.VideoListViewModel)DataContext;
            viewModel.SearchVideos();
        }
    }
}
