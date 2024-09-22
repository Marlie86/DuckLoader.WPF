using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Duckpond.WPF.Common.BaseClasses;
/// <summary>
/// Base class for view models that implements the INotifyPropertyChanged interface.
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets a value indicating whether an exception should be thrown when an invalid property name is used.
    /// </summary>
    public bool ThrowOnInvalidPropertyName { get; private set; } = true;

    /// <summary>
    /// Event that is raised when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event for the specified property.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler == null)
        {
            return;
        }
        var e = new PropertyChangedEventArgs(propertyName);
        handler(this, e);
    }

    /// <summary>
    /// Verifies that the specified property name matches a real, public, instance property on this object.
    /// </summary>
    /// <param name="propertyName">The name of the property to verify.</param>
    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public void VerifyPropertyName(string propertyName)
    {
        // Verify that the property name matches a real, 
        // public, instance property on this object. 
        if (TypeDescriptor.GetProperties(this)[propertyName] == null)
        {
            string msg = "Invalid property name: " + propertyName;
            if (ThrowOnInvalidPropertyName)
            {
                throw new Exception(msg);
            }
            else
            {
                Debug.Fail(msg);
            }
        }
    }
}
