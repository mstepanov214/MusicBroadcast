namespace MusicBroadcast
{
    class BroadcastException : Exception
    {
        public BroadcastException(Exception inner) : base("Broadcast exception", inner) { }

        public BroadcastException(string message) : base(message) { }
    }
}
