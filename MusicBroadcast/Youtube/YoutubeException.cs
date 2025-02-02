namespace MusicBroadcast.Youtube;

public abstract class YoutubeException : Exception
{
    protected YoutubeException(string message) : base(message) { }
}

public class YoutubeUnavailableException : YoutubeException
{
    public YoutubeUnavailableException(string url) : base($"{url} is unavailable.") { }

    public YoutubeUnavailableException(string url, string error) : base($"{url} is unavailable.\n{error}") { }
}

public class YoutubeCopyrightException : YoutubeException
{
    public YoutubeCopyrightException(string url) : base($"{url} is copyrighted") { }
}
