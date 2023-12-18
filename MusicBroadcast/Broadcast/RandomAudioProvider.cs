using MusicBroadcast.Extensions;
using MusicBroadcast.Parser;
using MusicBroadcast.Youtube;

namespace MusicBroadcast;

internal class RandomAudioProvider : IAudioProvider
{
    private readonly IBroadcastConfig _config;
    private readonly IParser<string[]> _parser;

    public RandomAudioProvider(IBroadcastConfig config, IParser<string[]> parser)
    {
        _config = config;
        _parser = parser;
    }

    public async IAsyncEnumerable<string> GetDataAsync()
    {
        while (true)
        {
            string audioUrl = await FetchValidAudioUrl();
            yield return audioUrl;
        }
    }

    private async Task<string> FetchValidAudioUrl()
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

        return await YoutubeDataClient.GetYoutubeData(youtubeUrl);
    }
}
