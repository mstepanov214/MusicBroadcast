using MusicBroadcast.Extensions;
using MusicBroadcast.Parser;
using MusicBroadcast.Youtube;

namespace MusicBroadcast;

internal class RandomAudioProvider : IAudioProvider
{
    private readonly IBroadcastConfig _config;
    private readonly IParser<LastfmParseResult> _parser;
    private int _pagesTotal = 1;

    public RandomAudioProvider(IBroadcastConfig config, IParser<LastfmParseResult> parser)
    {
        _config = config;
        _parser = parser;
    }

    public async IAsyncEnumerable<string> GetDataAsync()
    {
        while (true)
        {
            var youtubeData = await FetchValidYoutubeData();
            Console.WriteLine($"{youtubeData.Title} ({youtubeData.Url})");

            yield return youtubeData.AudioUrl;
        }
    }

    private async Task<YoutubeData> FetchValidYoutubeData()
    {
        YoutubeData? youtubeData = null;
        do
        {
            try
            {
                youtubeData = await GetRandomTrackYoutubeData();
            }
            catch (YoutubeException e)
            {
                Console.WriteLine(e.ToString());
            }
        } while (youtubeData == null);

        return youtubeData;
    }

    private async Task<YoutubeData> GetRandomTrackYoutubeData()
    {
        string tracksUrl = GetTracksUrl();
        var result = await _parser.Parse(tracksUrl);

        string youtubeUrl = result.Urls.GetRandomElement();
        _pagesTotal = result.PagesTotal;

        return await YoutubeDataClient.GetYoutubeData(youtubeUrl);
    }

    private string GetTracksUrl()
    {
        var uri = new Uri(_config.TracksUrl);
        if (!uri.IsWellFormedOriginalString())
        {
            throw new ArgumentException($"TracksUrl is not valid: {_config.TracksUrl}");
        }

        int pageNumber = new Random().Next(1, _pagesTotal + 1);
        return uri.AddParameter("page", pageNumber.ToString()).ToString();
    }
}
