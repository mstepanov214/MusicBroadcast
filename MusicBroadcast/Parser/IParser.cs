namespace MusicBroadcast.Parser
{
    interface IParser<T> where T : class
    {
        Task<T> Parse(string address);
    }
}
