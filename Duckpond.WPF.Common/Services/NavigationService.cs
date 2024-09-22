using Duckpond.WPF.Common.BaseClasses;


namespace Duckpond.WPF.Common.Services;
/// <summary>
/// Represents a navigation service that allows navigation to different targets.
/// </summary>
public class NavigationService : BaseNotifyPropertyChanged
{
    /// <summary>
    /// Represents the delegate for the NavigateTo event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="target">The target object to navigate to.</param>
    public delegate void NavigateToHandler(object sender, object target);

    /// <summary>
    /// Occurs when a navigation to a target is requested.
    /// </summary>
    public event NavigateToHandler OnNavigateToHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationService"/> class.
    /// </summary>
    public NavigationService()
    {
    }

    /// <summary>
    /// Navigates to the specified target.
    /// </summary>
    /// <param name="target">The target object to navigate to.</param>
    public void NavigateTo(object target)
    {
        OnNavigateToHandler?.Invoke(this, target);
    }
}
