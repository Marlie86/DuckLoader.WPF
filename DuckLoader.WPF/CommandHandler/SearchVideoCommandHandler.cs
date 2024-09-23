
using DuckLoader.WPF.Commands;
using DuckLoader.WPF.Models;

using Google.Apis.YouTube.v3;

using MediatR;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Search;

namespace DuckLoader.WPF.CommandHandler;

/// <summary>
/// Handles the search video command and returns a list of video search results.
/// </summary>
public class SearchVideoCommandHandler : IRequestHandler<SearchVideoCommand, (string, List<VideoSearchResultModel>)>
{
    private readonly IConfiguration configuration;

    public SearchVideoCommandHandler(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Handles the search video command and returns a list of video search results.
    /// </summary>
    /// <param name="request">The search video command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of video search results.</returns>
    public async Task<(string, List<VideoSearchResultModel>)> Handle(SearchVideoCommand request, CancellationToken cancellationToken)
    {
        if (configuration.GetValue<bool>("YoutubeApiSettings:UseYoutubeApi") == true)
        {
            //throw new Exception("Test");
            return await LoadViaYoutubeApi(request, cancellationToken);

        }
        else
        {
            return await LoadSlow(request, cancellationToken);


            // mock
            //var searchResults = new List<VideoSearchResultModel>();
            //var validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 -.,!\"'§&/(";
            //var random = new Random();
            //Enumerable.Range(0, 10).ToList().ForEach(i =>
            //{
            //    var randomLength = random.Next(5, 15);
            //    var randomTitle = new string(Enumerable.Range(0, randomLength).Select(_ => validChars[random.Next(validChars.Length)]).ToArray());

            //    searchResults.Add(new VideoSearchResultModel
            //    {
            //        Title = $"{randomTitle}",
            //        Author = $"Author {randomTitle}",
            //        Url = $"Url {randomTitle}",
            //        ThumbnailUrl = $"ThumbnailUrl {randomTitle}"
            //    });
            //});
            //await Task.Delay(20);
            //return ("ABCBDJ", searchResults);
        }
    }

    private async Task<(string, List<VideoSearchResultModel>)> LoadViaYoutubeApi(SearchVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var youtubeService = new YouTubeService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApiKey = configuration.GetValue<string>("YoutubeApiSettings:YoutubeApiKey")
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = request.VideoSearchTerm;
            searchListRequest.MaxResults = configuration.GetValue<int>("YoutubeApiSettings:YoutubeApiMaxResults");
            searchListRequest.Type = "video";
            searchListRequest.SafeSearch = SearchResource.ListRequest.SafeSearchEnum.None;
            searchListRequest.PageToken = request.PageToken;

            var searchListResponse = searchListRequest.Execute();

            var results = new List<VideoSearchResultModel>();
            searchListResponse.Items.ToList().ForEach(item =>
            {
                var videoSearchResult = new VideoSearchResultModel
                {
                    Title = item.Snippet.Title,
                    Author = item.Snippet.ChannelTitle,
                    Url = item.Id.VideoId,
                    ThumbnailUrl = item.Snippet.Thumbnails.Default__.Url,
                    IsDownloading = System.Windows.Visibility.Collapsed
                };
                results.Add(videoSearchResult);
            });
            var pageToken = string.IsNullOrEmpty(searchListResponse.NextPageToken) ? "end" : searchListResponse.NextPageToken;

            return (searchListResponse.NextPageToken, results);
        }
        catch
        {
            return await LoadSlow(request, cancellationToken);
        }
    }

    private static async Task<(string, List<VideoSearchResultModel>)> LoadSlow(SearchVideoCommand request, CancellationToken cancellationToken)
    {
        var results = new List<VideoSearchResultModel>();
        var youtubeClient = new YoutubeClient();
        var loadingTask = await youtubeClient.Search.GetVideosAsync(request.VideoSearchTerm, cancellationToken);
        results.AddRange(loadingTask.Select(item => new VideoSearchResultModel
        {
            Title = item.Title,
            Author = item.Author.ChannelTitle,
            Url = item.Url,
            ThumbnailUrl = item.Thumbnails.Count > 0 ? item.Thumbnails[0].Url : string.Empty,
            IsDownloading = System.Windows.Visibility.Collapsed
        }));
        return ("end", results);
    }
}
