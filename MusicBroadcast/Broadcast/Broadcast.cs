using MusicBroadcast.Converter;

namespace MusicBroadcast;

class Broadcast
{
    private readonly IBroadcastConfig _config;
    private readonly IConverter _converter;
    private readonly IAudioProvider _randomAudioProvider;

    public Broadcast(IBroadcastConfig config, IConverter converter, IAudioProvider randomAudioProvider)
    {
        _config = config;
        _converter = converter;
        _randomAudioProvider = randomAudioProvider;
    }

    public async Task Start(CancellationToken ct = default)
    {
        await foreach (var audioUrl in _randomAudioProvider.GetDataAsync())
        {
            try
            {
                await _converter.Convert(audioUrl, _config.OutputUrl, ct);
            }
            catch (BroadcastException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
