﻿using MusicBroadcast.Extensions;

using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;
using YoutubeDLSharp.Options;

namespace MusicBroadcast.Youtube;

internal static class YoutubeDataClient
{
    static readonly YoutubeDL _ytdl = new();

    public static async Task<YoutubeData> GetYoutubeData(string url, CancellationToken ct = default)
    {
        var options = new OptionSet()
        {
            ExtractorArgs = "youtube:skip=dash"
        };

        var videoDataFetch = await _ytdl.RunVideoDataFetch(url, ct, overrideOptions: options);

        if (!videoDataFetch.Success)
        {
            throw new YoutubeUnavailableException(url);
        }

        if (IsCopyrighted(videoDataFetch.Data))
        {
            throw new YoutubeCopyrightException(url);
        }

        return CreateYoutubeData(videoDataFetch.Data);
    }

    private static bool IsCopyrighted(VideoData data)
    {
        if (data.License.IsNotEmpty()) return true;

        var fields = new[]{
            data.Uploader,
            data.UploaderUrl,
            data.UploaderID,
            data.Channel,
            data.Description,
            string.Join(' ', data.Tags)
        };
        return fields.Any(field => field?.Contains("vevo", StringComparison.OrdinalIgnoreCase) ?? false);
    }

    private static YoutubeData CreateYoutubeData(VideoData videoData)
    {
        return new YoutubeData()
        {
            AudioUrl = videoData.Formats.First(format => format.FormatId == "140").Url,
            Title = videoData.Title,
            Url = videoData.WebpageUrl,
            Duration = videoData.Duration == null ? null : TimeSpan.FromSeconds((double)videoData.Duration)
        };
    }
}
