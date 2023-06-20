using Extensions;

using MusicBroadcast.Converter;
using MusicBroadcast.Exceptions;
using MusicBroadcast.Parser;

namespace MusicBroadcast
{
    class Broadcast
    {
        private IParser<string[]> Parser { get; init; }

        private IConverter Converter { get; init; }

        private IBroadcastConfig Config { get; init; }

        private CancellationToken _ct;

        private string? _nextAudioUrl;

        public Broadcast(IBroadcastConfig config, IParser<string[]> parser, IConverter converter)
        {
            Config = config;
            Parser = parser;
            Converter = converter;

        }

        public void Start(CancellationToken ct = default)
        {
            _ct = ct;

            Converter.ThresholdReached += async (object? sender, EventArgs e) =>
            {
                _nextAudioUrl = await FetchNextAudioUrl();
            };

            _nextAudioUrl = FetchNextAudioUrl().GetAwaiter().GetResult();

            while (!_ct.IsCancellationRequested)
            {
                try
                {
                    Converter.Convert(_nextAudioUrl, Config.OutputUrl, _ct).GetAwaiter().GetResult();
                }
                catch (BroadcastException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private async Task<string> FetchNextAudioUrl()
        {
            YoutubeData? youtubeData = null;
            do
            {
                try
                {
                    youtubeData = await FetchRandomTrackData();
                }
                catch (YoutubeException e)
                {
                    Console.WriteLine(e.ToString());
                }
            } while (youtubeData == null);

            Console.WriteLine($"{youtubeData.Title} ({youtubeData.Url})");
            return youtubeData.AudioUrl;
        }

        private async Task<YoutubeData> FetchRandomTrackData()
        {
            string tracksUrl = Config.TracksUrl + $"?page={new Random().Next(1, Config.PagesTotal + 1)}";
            string youtubeUrl = (await Parser.Parse(tracksUrl)).GetRandomElement();

            return await YoutubeData.Load(youtubeUrl, _ct);
        }
    }
}
