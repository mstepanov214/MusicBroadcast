namespace MusicBroadcast.Youtube
{
    abstract class YoutubeException : Exception
    {
        protected YoutubeException(string message) : base(message) { }
    }

    class YoutubeUnavailableException : YoutubeException
    {
        public YoutubeUnavailableException(string url) : base($"{url} is unavailable") { }
    }

    class YoutubeCopyrightException : YoutubeException
    {
        public YoutubeCopyrightException(string url) : base($"{url} is copyrighted") { }
    }
}
