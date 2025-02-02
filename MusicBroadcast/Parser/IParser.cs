namespace MusicBroadcast.Parser;

public interface IParser<T> where T : class
{
    Task<T> Parse(string address);
}
