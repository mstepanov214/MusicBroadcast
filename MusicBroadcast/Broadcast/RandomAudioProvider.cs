using MusicBroadcast.Extensions;
using MusicBroadcast.Parser;
using MusicBroadcast.Youtube;

namespace MusicBroadcast;

internal class RandomAudioProvider : IAudioSourceProvider
{
    private readonly IBroadcastConfig _config;
    private readonly IParser<string[]> _parser;
    private readonly IYoutubeDataProvider _youtubeDataProvider;

    public RandomAudioProvider(IBroadcastConfig config, IParser<string[]> parser, IYoutubeDataProvider youtubeDataProvider)
    {
        _config = config;
        _parser = parser;
        _youtubeDataProvider = youtubeDataProvider;
    }

    public async Task<string> GetNext()
    {
        YoutubeData? youtubeData = null;
        do
        {
            try
            {
                youtubeData = await GetRandomTrackData();
            }
            catch (YoutubeException e)
            {
                Console.WriteLine(e.ToString());
            }
        } while (youtubeData == null);

        Console.WriteLine($"{youtubeData.Title} ({youtubeData.Url})");
        return youtubeData.AudioUrl;
    }

    private async Task<YoutubeData> GetRandomTrackData()
    {
        int randomPage = new Random().Next(1, _config.PagesTotal + 1);
        string tracksUrl = _config.TracksUrl + $"?page={randomPage}";
        string youtubeUrl = (await _parser.Parse(tracksUrl)).GetRandomElement();

        return await _youtubeDataProvider.GetYoutubeData(youtubeUrl);
    }
}
