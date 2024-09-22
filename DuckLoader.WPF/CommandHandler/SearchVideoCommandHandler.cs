using DuckLoader.WPF.Commands;
using DuckLoader.WPF.Models;

using Google.Apis.YouTube.v3;

using MediatR;

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
    /// <summary>
    /// Handles the search video command and returns a list of video search results.
    /// </summary>
    /// <param name="request">The search video command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of video search results.</returns>
    public async Task<(string, List<VideoSearchResultModel>)> Handle(SearchVideoCommand request, CancellationToken cancellationToken)
    {
        //var youtube = new YoutubeClient();
        //var searchResults = await youtube.Search.GetVideosAsync(request.VideoSearchTerm, cancellationToken);

        //return searchResults.ToList();

        
        var youtubeService = new YouTubeService(new Google.Apis.Services.BaseClientService.Initializer()
        {
            ApiKey = "AIzaSyDcSmp-0l0eZpBetqvBkjUQhXrY7L4iziA"
        });

        var searchListRequest = youtubeService.Search.List("snippet");
        searchListRequest.Q = request.VideoSearchTerm;
        searchListRequest.MaxResults = 50;
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
                ThumbnailUrl = item.Snippet.Thumbnails.Default__.Url
            };
            results.Add(videoSearchResult);
        });

        return (searchListResponse.NextPageToken, results);    
    }
}
