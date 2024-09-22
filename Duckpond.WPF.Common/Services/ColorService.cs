using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Duckpond.WPF.Common.Services;
/// <summary>
/// Provides a service for retrieving colors.
/// </summary>
public class ColorService : DynamicObject
{
    /// <summary>
    /// Tries to get the color with the specified name or hex code.
    /// </summary>
    /// <param name="binder">The binder representing the member being accessed.</param>
    /// <param name="result">The color associated with the specified name, if found; otherwise, a default image.</param>
    /// <returns><c>true</c> if the color with the specified name is found; otherwise, <c>false</c>.</returns>
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        var icon = GetColor(binder.Name);
        if (icon != null)
        {
            result = icon;
            return true;
        }
        result = new System.Windows.Controls.Image();
        return false;
    }

    /// <summary>
    /// Gets the color based on the specified color codeor name.
    /// </summary>
    /// <param name="colorCode">The name or hex code of the color.</param>
    /// <returns>The color as a SolidColorBrush.</returns>
    private object GetColor(string colorCode)
    {
        Color color;
        if (colorCode.StartsWith("#"))
        {
            color = (Color)ColorConverter.ConvertFromString(colorCode);
        }
        else
        {
            var properties = typeof(Colors).GetProperties();
            var colorProperty = properties.FirstOrDefault(p => p.Name.Equals(colorCode, StringComparison.InvariantCultureIgnoreCase));

            color = (Color)colorProperty.GetValue(null);
        }
        var brush = new SolidColorBrush(color);
        return brush;
    }
}
