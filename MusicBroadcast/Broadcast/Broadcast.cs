using MusicBroadcast.Converter;

namespace MusicBroadcast;

class Broadcast
{
    private readonly IBroadcastConfig _config;
    private readonly IConverter _converter;
    private readonly IAudioSourceProvider _randomAudioProvider;

    private CancellationToken _ct;

    public Broadcast(IBroadcastConfig config, IConverter converter, IAudioSourceProvider randomAudioProvider)
    {
        _config = config;
        _converter = converter;
        _randomAudioProvider = randomAudioProvider;
    }

    public async Task Start(CancellationToken ct = default)
    {
        _ct = ct;
        string? audioUrl;

        while (!_ct.IsCancellationRequested)
        {
            try
            {
                audioUrl = await _randomAudioProvider.GetNext();
                await _converter.Convert(audioUrl, _config.OutputUrl, _ct);
            }
            catch (BroadcastException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
