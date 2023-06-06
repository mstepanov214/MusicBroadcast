
using Extensions;

using MusicBroadcast.Exceptions;

using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;
using YoutubeDLSharp.Options;

namespace MusicBroadcast
{
    class YoutubeData
    {
        // yt-dlp 2023.3.4.0
        static readonly YoutubeDL _ytdl = new();
        private readonly string _audioUrl;
        private readonly string _title;
        private readonly string _url;
        private readonly TimeSpan? _duration;

        private YoutubeData(VideoData videoData)
        {
            _audioUrl = videoData.Formats.First(format => format.FormatId == "140").Url;
            _title = videoData.Title;
            _url = videoData.WebpageUrl;
            _duration = videoData.Duration == null ? null : TimeSpan.FromSeconds((double)videoData.Duration);
        }

        public string AudioUrl { get => _audioUrl; }

        public string Title { get => _title; }

        public string Url { get => _url; }

        public TimeSpan? Duration => _duration;

        public async static Task<YoutubeData> Load(string url, CancellationToken ct = default)
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

            return new YoutubeData(videoDataFetch.Data);
        }

        private static bool IsCopyrighted(VideoData data)
        {
            if (data.License.IsNotEmpty()) return true;

            var fields = new string[]{
                data.Uploader,
                data.UploaderUrl,
                data.UploaderID,
                data.Channel,
                data.Description,
            };
            if (fields.Any(field => field.Contains("vevo", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
            return data.Tags.Contains("vevo", StringComparer.OrdinalIgnoreCase);
        }
    }
}
