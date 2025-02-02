using YoutubeDLSharp.Metadata;

namespace MusicBroadcast.Youtube;

public class YoutubeData
{
    public string AudioUrl { get; }

    public string Title { get; }

    public string Url { get; }

    public TimeSpan? Duration { get; }

    public YoutubeData(VideoData videoData)
    {
        var formatData = videoData.Formats.FirstOrDefault(
            data => data.FormatId == "140",
            defaultValue: videoData.Formats[0]);

        AudioUrl = formatData.Url;
        Title = videoData.Title;
        Url = videoData.WebpageUrl;
        Duration = videoData.Duration == null ? null : TimeSpan.FromSeconds((double)videoData.Duration);
    }
}
