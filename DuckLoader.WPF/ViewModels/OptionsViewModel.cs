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
public class OptionsViewModel : BaseViewModel
{
    private IConfiguration configuration;
    private ICommand openDirectoryPicker;

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

    public ICommand OpenDirectoryPicker { 
        get {
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

    public OptionsViewModel(IConfiguration configuration)
    {
        this.configuration = configuration;
        SaveSettings();
    }

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
