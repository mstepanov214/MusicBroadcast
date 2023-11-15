namespace MusicBroadcast;

class YoutubeData
{
    public required string AudioUrl { get; set; }

    public required string Title { get; set; }

    public required string Url { get; set; }

    public TimeSpan? Duration { get; set; }
}
