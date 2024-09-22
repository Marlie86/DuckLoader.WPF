using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckLoader.WPF.Commands;
/// <summary>
/// Represents a command to load a video.
/// </summary>
public class LoadVideoCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the URL of the video.
    /// </summary>
    public string VideoUrl { get; set; }
}
