using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Duckpond.WPF.Common.Utilities;
/// <summary>
/// Represents a utility class that enables ElementName and DataContext bindings.
/// </summary>
public class DataContextSpy : Freezable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataContextSpy"/> class.
    /// </summary>
    public DataContextSpy()
    {
        // This binding allows the spy to inherit a DataContext.
        BindingOperations.SetBinding(this, DataContextProperty, new Binding());

        this.IsSynchronizedWithCurrentItem = true;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the spy will return the CurrentItem of the 
    /// ICollectionView that wraps the data context, assuming it is
    /// a collection of some sort. If the data context is not a 
    /// collection, this property has no effect. 
    /// The default value is true.
    /// </summary>
    public bool IsSynchronizedWithCurrentItem { get; set; }

    /// <summary>
    /// Gets or sets the data context.
    /// </summary>
    public object DataContext
    {
        get { return (object)GetValue(DataContextProperty); }
        set { SetValue(DataContextProperty, value); }
    }

    /// <summary>
    /// Identifies the DataContext dependency property.
    /// </summary>
    public static readonly DependencyProperty DataContextProperty =
        FrameworkElement.DataContextProperty.AddOwner(
        typeof(DataContextSpy),
        new PropertyMetadata(null, null, OnCoerceDataContext));

    /// <summary>
    /// Coerces the data context value based on the synchronization mode.
    /// </summary>
    /// <param name="depObj">The dependency object.</param>
    /// <param name="value">The original data context value.</param>
    /// <returns>The coerced data context value.</returns>
    private static object OnCoerceDataContext(DependencyObject depObj, object value)
    {
        DataContextSpy spy = depObj as DataContextSpy;
        if (spy == null)
            return value;

        if (spy.IsSynchronizedWithCurrentItem)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(value);
            if (view != null)
                return view.CurrentItem;
        }

        return value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DataContextSpy"/> class.
    /// </summary>
    /// <returns>A new instance of the <see cref="DataContextSpy"/> class.</returns>
    protected override Freezable CreateInstanceCore()
    {
        // We are required to override this abstract method.
        throw new NotImplementedException();
    }
}
