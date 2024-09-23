using Duckpond.WPF.Common.BaseClasses;
using Duckpond.WPF.Common.Utilities;

using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DuckLoader.WPF.ViewModels;
/// <summary>
/// Represents the view model for the options view.
/// </summary>
public class OptionsViewModel : BaseViewModel
{
    private IConfiguration configuration;
    private ICommand openDirectoryPicker;

    /// <summary>
    /// Gets or sets a value indicating whether to use the YouTube API.
    /// </summary>
    public bool UseYoutubeApi
    {
        get
        {
            return configuration.GetValue<bool>("YoutubeApiSettings:UseYoutubeApi");
        }
        set
        {
            configuration["YoutubeApiSettings:UseYoutubeApi"] = value.ToString();
            OnPropertyChanged();
            SaveSettings();
        }
    }

    /// <summary>
    /// Gets or sets the YouTube API key.
    /// </summary>
    public string YoutubeApiKey
    {
        get
        {
            return configuration.GetValue<string>("YoutubeApiSettings:YoutubeApiKey") ?? string.Empty;
        }
        set
        {
            configuration["YoutubeApiSettings:YoutubeApiKey"] = value;
            OnPropertyChanged();
            SaveSettings();
        }
    }

    /// <summary>
    /// Gets or sets the download directory.
    /// </summary>
    public string DownloadDirectory
    {
        get
        {
            return configuration.GetValue<string>("DownloadDirectory") ?? string.Empty;
        }
        set
        {
            configuration["DownloadDirectory"] = value;
            OnPropertyChanged();
            SaveSettings();
        }
    }

    /// <summary>
    /// Gets the command to open the directory picker.
    /// </summary>
    public ICommand OpenDirectoryPicker
    {
        get
        {
            return new RelayCommand<string>(p =>
            {
                var folderOptionsDialog = new OpenFolderDialog()
                {
                    Title = "Select download directory"
                };

                if (folderOptionsDialog.ShowDialog() == true)
                {
                    DownloadDirectory = folderOptionsDialog.FolderName;
                }
            });
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionsViewModel"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public OptionsViewModel(IConfiguration configuration)
    {
        this.configuration = configuration;
        SaveSettings();
    }

    /// <summary>
    /// Saves the settings to the configuration file.
    /// </summary>
    private void SaveSettings()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        var configContent = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(filePath));

        configContent["YoutubeApiSettings"]["UseYoutubeApi"] = configuration.GetValue<bool>("YoutubeApiSettings:UseYoutubeApi");
        configContent["YoutubeApiSettings"]["YoutubeApiKey"] = configuration.GetValue<string>("YoutubeApiSettings:YoutubeApiKey");
        configContent["DownloadDirectory"] = configuration.GetValue<string>("DownloadDirectory");
        configContent["FirstTime"] = false;

        var jsonString = JsonConvert.SerializeObject(configContent, Formatting.Indented);
        File.WriteAllText(filePath, jsonString);
    }
}
