using MusicBroadcast.Extensions;

using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;
using YoutubeDLSharp.Options;

namespace MusicBroadcast.Youtube;

internal static class YoutubeDataClient
{
    static readonly YoutubeDL _ytdl = new();
    private const string _cookiesPath = "cookies.txt";

    public static async Task<YoutubeData> GetYoutubeData(string url, CancellationToken ct = default)
    {
        if (!File.Exists(_cookiesPath))
        {
            throw new FileNotFoundException("YouTube cookies file was not found.", _cookiesPath);
        }

        var options = new OptionSet()
        {
            ExtractorArgs = "youtube:skip=dash",
            Cookies = _cookiesPath
        };

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
        var formatData = videoData.Formats.FirstOrDefault(
            data => data.FormatId == "140",
            defaultValue: videoData.Formats[0]);

        return new YoutubeData()
        {
            AudioUrl = formatData.Url,
            Title = videoData.Title,
            Url = videoData.WebpageUrl,
            Duration = videoData.Duration == null ? null : TimeSpan.FromSeconds((double)videoData.Duration)
        };
    }
}
