namespace MusicBroadcast;

interface IAudioSourceProvider
{
    /// <summary>
    /// Provides audio source string
    /// </summary>
    Task<string> GetNext();
}

