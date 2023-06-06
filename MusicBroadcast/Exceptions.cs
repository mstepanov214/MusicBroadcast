namespace MusicBroadcast.Exceptions
{
    class BroadcastException : Exception
    {
        public BroadcastException(Exception inner) : base("Broadcast exception", inner) { }

        public BroadcastException(String message) : base(message) { }
    }

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
