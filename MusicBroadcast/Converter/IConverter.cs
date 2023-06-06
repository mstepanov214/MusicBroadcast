namespace MusicBroadcast.Converter
{
    interface IConverter
    {
        Task Convert(string input, string output, CancellationToken ct = default);

        event EventHandler ThresholdReached;
    }
}
