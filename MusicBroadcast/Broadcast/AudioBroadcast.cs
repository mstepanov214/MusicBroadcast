using MusicBroadcast.Converter;

namespace MusicBroadcast.Broadcast;

public class AudioBroadcast
{
    private readonly IBroadcastConfig _config;
    private readonly IConverter _converter;
    private readonly IAudioProvider _randomAudioProvider;
    private const int _maxFailCount = 5;

    public AudioBroadcast(IBroadcastConfig config, IConverter converter, IAudioProvider randomAudioProvider)
    {
        _config = config;
        _converter = converter;
        _randomAudioProvider = randomAudioProvider;
    }

    public async Task Start(CancellationToken ct = default)
    {
        int fails = 0;

        await foreach (var audio in _randomAudioProvider.GetDataAsync())
        {
            try
            {
                Console.WriteLine(audio.Description);
                await _converter.Convert(audio.Url, _config.OutputUrl, ct);
                fails = 0;
            }
            catch (ConverterException e) when (++fails < _maxFailCount)
            {
                Console.WriteLine(e);
            }
        }
    }
}
