using MusicBroadcast.Converter;

namespace MusicBroadcast.Broadcast;

public class AudioBroadcast
{
    private readonly IBroadcastConfig _config;
    private readonly IConverter _converter;
    private readonly IAudioProvider _randomAudioProvider;

    public AudioBroadcast(IBroadcastConfig config, IConverter converter, IAudioProvider randomAudioProvider)
    {
        _config = config;
        _converter = converter;
        _randomAudioProvider = randomAudioProvider;
    }

    public async Task Start(CancellationToken ct = default)
    {
        await foreach (var audio in _randomAudioProvider.GetDataAsync())
        {
            try
            {
                Console.WriteLine(audio.Description);
                await _converter.Convert(audio.Url, _config.OutputUrl, ct);
            }
            catch (ConverterException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
