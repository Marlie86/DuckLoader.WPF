using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using YoutubeExplode.Common;

namespace Duckpond.WPF.Common.Converters;

/// <summary>
/// Converts a URL string to a BitmapImage.
/// </summary>
[ValueConversion(typeof(string), typeof(BitmapImage))]
public class UrlToImageConverter : IValueConverter
{
    /// <summary>
    /// Converts a URL string to a BitmapImage.
    /// </summary>
    /// <param name="value">The URL string.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>The converted BitmapImage.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var thumbnail = value as Thumbnail;
        if (thumbnail == null)
        {
            return null;
        }
        var image = new Image();
        BitmapImage bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(thumbnail.Url);
        bitmap.EndInit();
        return bitmap;
    }

    /// <summary>
    /// Returns an empty string, back coversion is not supported.
    /// </summary>
    /// <param name="value">The BitmapImage.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>An empty string.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}
