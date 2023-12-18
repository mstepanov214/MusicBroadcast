namespace MusicBroadcast;

interface IAudioProvider
{
    /// <summary>
    /// Provides audio sources
    /// </summary>
    IAsyncEnumerable<string> GetDataAsync();
}