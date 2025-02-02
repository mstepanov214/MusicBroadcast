using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;
using YoutubeDLSharp.Options;

namespace MusicBroadcast.Youtube;

public static class YoutubeDataClient
{
    static readonly YoutubeDL _ytdl = new();

    public static async Task<YoutubeData> GetYoutubeData(string url, CancellationToken ct = default)
    {
        var options = new OptionSet()
        {
            ExtractorArgs = "youtube:skip=dash"
        };

        if (File.Exists(FilePaths.YoutubeCookies))
        {
            options.Cookies = FilePaths.YoutubeCookies;
        }

        var videoDataFetch = await _ytdl.RunVideoDataFetch(url, ct, overrideOptions: options);

        if (!videoDataFetch.Success)
        {
            string error = string.Join(Environment.NewLine, videoDataFetch.ErrorOutput);
            throw new YoutubeUnavailableException(url, error);
        }

        if (IsCopyrighted(videoDataFetch.Data))
        {
            throw new YoutubeCopyrightException(url);
        }

        return new YoutubeData(videoDataFetch.Data);
    }

    private static bool IsCopyrighted(VideoData data)
    {
        if (!string.IsNullOrEmpty(data.License)) return true;

        string[] fields = [
            data.Uploader,
            data.UploaderUrl,
            data.UploaderID,
            data.Channel,
            data.Description,
            ..data.Tags
        ];
        return fields.Any(field => field?.Contains("vevo", StringComparison.OrdinalIgnoreCase) ?? false);
    }
}
