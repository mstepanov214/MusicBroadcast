namespace MusicBroadcast.Parser;

public class ParserException : Exception
{
    public ParserException(Exception inner) : base("Parser exception", inner) { }
}
