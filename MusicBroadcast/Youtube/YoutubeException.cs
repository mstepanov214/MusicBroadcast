namespace MusicBroadcast.Youtube;

abstract class YoutubeException : Exception
{
    protected YoutubeException(string message) : base(message) { }
}

class YoutubeUnavailableException : YoutubeException
{
    public YoutubeUnavailableException(string url) : base($"{url} is unavailable.") { }

    public YoutubeUnavailableException(string url, string error) : base($"{url} is unavailable.\n{error}") { }
}

class YoutubeCopyrightException : YoutubeException
{
    public YoutubeCopyrightException(string url) : base($"{url} is copyrighted") { }
}
