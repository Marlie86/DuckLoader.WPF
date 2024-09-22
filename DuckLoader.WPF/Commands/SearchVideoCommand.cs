using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YoutubeExplode.Search;

namespace DuckLoader.WPF.Commands;
/// <summary>
/// Represents a command to search for videos.
/// </summary>
public class SearchVideoCommand : IRequest<List<VideoSearchResult>>
{
    /// <summary>
    /// Gets or sets the search term for the video.
    /// </summary>
    public string VideoSearchTerm { get; set; } = string.Empty;
}
