using DuckLoader.WPF.Commands;
using DuckLoader.WPF.Models;

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
public class SearchVideoCommandHandler : IRequestHandler<SearchVideoCommand, List<VideoSearchResult>>
{
    /// <summary>
    /// Handles the search video command and returns a list of video search results.
    /// </summary>
    /// <param name="request">The search video command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of video search results.</returns>
    public async Task<List<VideoSearchResult>> Handle(SearchVideoCommand request, CancellationToken cancellationToken)
    {
        var youtube = new YoutubeClient();
        var searchQuery = new VideoSearchModel() { MaxResults = 10, Q = request.VideoSearchTerm };
        var searchResults = await youtube.Search.GetVideosAsync(request.VideoSearchTerm, cancellationToken);

        return searchResults.ToList();
    }
}
