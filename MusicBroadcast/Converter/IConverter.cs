namespace MusicBroadcast.Converter;

public interface IConverter
{
    Task Convert(string input, string output, CancellationToken ct = default);
}
