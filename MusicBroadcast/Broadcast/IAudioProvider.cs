namespace MusicBroadcast.Broadcast;

public interface IAudioProvider
{
    /// <summary>
    /// Provides audio sources
    /// </summary>
    IAsyncEnumerable<Audio> GetDataAsync();
}

public record Audio(string Url, string Description);