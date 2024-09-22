using Duckpond.WPF.Common.Models;
using Duckpond.WPF.Common.Utilities;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Svg;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Duckpond.WPF.Common.Services;
/// <summary>
/// Service for loading icons dynamically.
/// </summary>
public class IconLoaderService : DynamicObject, INotifyPropertyChanged
{
    private readonly IConfiguration configuration;

    private IconSettingsModel iconSettings = new IconSettingsModel();
    private Dictionary<string, Bitmap> Icons = new Dictionary<string, Bitmap>();

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="IconLoaderService"/> class.
    /// </summary>
    public IconLoaderService()
    {
        this.configuration = AppStatics.ServiceProvider.GetRequiredService<IConfiguration>();
        iconSettings = configuration.GetSection("IconSettings").Get<IconSettingsModel>();
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        var icon = GetIcon(binder.Name);
        if (icon != null)
        {
            result = icon;
            return true;
        }
        result = new System.Windows.Controls.Image();
        return false;
    }

    /// <summary>
    /// Gets the icon with the specified name.
    /// </summary>
    /// <param name="iconName">The name of the icon.</param>
    /// <returns>The icon as an object.</returns>
    public object GetIcon(string iconName)
    {
        if (string.IsNullOrEmpty(iconSettings.IconTheme))
        {
            return new object();
        }

        if (string.IsNullOrEmpty(iconSettings.IconPaths))
        {
            return new object();
        }

        int size = 24;
        var splitted = Regex.Split(iconName, ":");
        var colorCode = "#000000";
        if (splitted.Length > 1)
        {
            iconName = splitted[0];
            size = int.Parse(splitted[1]);
        }
        if (splitted.Length > 2)
        {
            colorCode = splitted[2];
        }

        if (!Icons.TryGetValue(iconName, out Bitmap? svgImage))
        {
            var colorConverter = new System.Drawing.ColorConverter();
            System.Drawing.Color color;
            if (colorCode.StartsWith("#"))
            {
                color = (System.Drawing.Color)colorConverter.ConvertFromString(colorCode);
            }
            else
            {
                var properties = typeof(System.Drawing.Color).GetProperties();
                var colorProperty = properties.FirstOrDefault(p => p.Name.Equals(colorCode, StringComparison.InvariantCultureIgnoreCase));

                color = (System.Drawing.Color)colorProperty.GetValue(null);
            }
            var paintServer = new SvgColourServer(color);
            var svgDoc = SvgDocument.Open($"{iconSettings.IconPaths}/{iconSettings.IconTheme}/{iconName}.svg");
            svgDoc.Height = size;
            svgDoc.Width = size;
            svgDoc.ApplyRecursive((element) =>
            {
                element.Fill = paintServer;
                element.FillOpacity = 1.0f;
            });

            svgImage = svgDoc.Draw();
            Icons.Add(iconName, svgImage);

        }

        var image = new System.Windows.Controls.Image();
        var handle = svgImage.GetHbitmap();
        image.Source = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        image.Height = size;
        image.Width = size;

        return image;
    }
}
