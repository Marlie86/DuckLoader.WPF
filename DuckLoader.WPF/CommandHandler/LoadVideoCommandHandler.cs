using AngleSharp;

using DuckLoader.WPF.Commands;

using MediatR;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using YoutubeExplode;

namespace DuckLoader.WPF.CommandHandler;
/// <summary>
/// Handles the command to load a video from YouTube.
/// </summary>
public class LoadVideoCommandHandler(
    Microsoft.Extensions.Configuration.IConfiguration configuration
) : IRequestHandler<LoadVideoCommand, bool>
{
    /// <summary>
    /// Handles the load video command and downloads the video from YouTube.
    /// </summary>
    /// <param name="request">The load video command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the video is loaded successfully, otherwise false.</returns>
    public async Task<bool> Handle(LoadVideoCommand request, CancellationToken cancellationToken)
    {
        var downloadDirectory = configuration.GetValue<string>("DownloadDirectory");
        if (Directory.Exists(downloadDirectory) == false)
        {
            Directory.CreateDirectory(downloadDirectory);
        }

        var youtube = new YoutubeClient();
        var video = await youtube.Videos.GetAsync(request.VideoUrl);

        string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));

        // Get all available muxed streams
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
        var muxedStreams = streamManifest.GetMuxedStreams().OrderByDescending(s => s.VideoQuality).ToList();

        if (muxedStreams.Any())
        {
            var streamInfo = muxedStreams.First();
            using var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(streamInfo.Url);
            var datetime = DateTime.Now;

            string outputFilePath = Path.Combine(downloadDirectory, $"{sanitizedTitle}.{streamInfo.Container}");

            using var outputStream = File.Create(outputFilePath);
            await stream.CopyToAsync(outputStream);

            return true;
        }
        else
        {
            return false;
        }

    }
}
