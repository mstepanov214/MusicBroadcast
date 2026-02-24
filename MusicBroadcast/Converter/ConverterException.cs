namespace MusicBroadcast.Converter;

public class ConverterException : Exception
{
    public ConverterException(string message) : base(message) { }

    public ConverterException(string message, Exception inner) : base(message, inner) { }

}
