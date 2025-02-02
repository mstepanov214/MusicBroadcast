namespace MusicBroadcast.Converter;

public class ConverterException : Exception
{
    public ConverterException(Exception inner) : base("Converter exception", inner) { }

    public ConverterException(string message) : base(message) { }
}
