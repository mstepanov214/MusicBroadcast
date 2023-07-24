namespace MusicBroadcast.Youtube
{
    interface IYoutubeDataProvider
    {
        public Task<YoutubeData> GetYoutubeData(string url, CancellationToken ct = default);
    }
}
