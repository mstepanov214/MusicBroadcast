﻿using MusicBroadcast.Converter;
using MusicBroadcast.Extensions;
using MusicBroadcast.Parser;
using MusicBroadcast.Youtube;

namespace MusicBroadcast
{
    class Broadcast
    {
        private readonly IBroadcastConfig _config;
        private readonly IParser<string[]> _parser;
        private readonly IConverter _converter;
        private readonly IYoutubeDataProvider _youtubeDataProvider;

        private CancellationToken _ct;

        public Broadcast(IBroadcastConfig config, IParser<string[]> parser, IConverter converter, IYoutubeDataProvider youtubeDataProvider)
        {
            _config = config;
            _parser = parser;
            _converter = converter;
            _youtubeDataProvider = youtubeDataProvider;
        }

        public void Start(CancellationToken ct = default)
        {
            _ct = ct;
            string? nextAudioUrl;

            _converter.ThresholdReached += async (object? sender, EventArgs e) =>
            {
                nextAudioUrl = await FetchNextAudioUrl();
            };

            nextAudioUrl = FetchNextAudioUrl().GetAwaiter().GetResult();

            while (!_ct.IsCancellationRequested)
            {
                try
                {
                    _converter.Convert(nextAudioUrl, _config.OutputUrl, _ct).GetAwaiter().GetResult();
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
            string tracksUrl = _config.TracksUrl + $"?page={new Random().Next(1, _config.PagesTotal + 1)}";
            string youtubeUrl = (await _parser.Parse(tracksUrl)).GetRandomElement();

            return await _youtubeDataProvider.GetYoutubeData(youtubeUrl, _ct);
        }
    }
}
