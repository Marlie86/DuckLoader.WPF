using Duckpond.WPF.Common.BaseClasses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckLoader.WPF.Utilities;

/// <summary>
/// Represents the session context for the DuckLoader application.
/// </summary>
public class SessionContext : BaseNotifyPropertyChanged
{
    private string videoSearch = "";

    /// <summary>
    /// Gets or sets the video search term.
    /// </summary>
    public string VideoSearch { get => videoSearch; set { videoSearch = value; OnPropertyChanged(); } }
}
