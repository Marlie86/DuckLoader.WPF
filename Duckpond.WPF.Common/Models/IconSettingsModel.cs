using Duckpond.WPF.Common.BaseClasses;

namespace Duckpond.WPF.Common.Models;
/// <summary>
/// Represents the model for icon settings.
/// </summary>
public class IconSettingsModel : BaseNotifyPropertyChanged
{
    private string iconTheme = string.Empty;

    /// <summary>
    /// Gets or sets the icon theme.
    /// </summary>
    public string IconTheme { get => iconTheme; set { iconTheme = value; OnPropertyChanged(); } }

    private string iconPaths = string.Empty;

    /// <summary>
    /// Gets or sets the icon paths.
    /// </summary>
    public string IconPaths { get => iconPaths; set { iconPaths = value; OnPropertyChanged(); } }
}
