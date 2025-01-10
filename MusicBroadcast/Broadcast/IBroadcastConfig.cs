namespace MusicBroadcast;

interface IBroadcastConfig
{
    /// <summary>
    /// Stream output url
    /// </summary>
    string OutputUrl { get; }

    /// <summary>
    /// Url to parse tracks data from
    /// </summary>
    string TracksUrl { get; }
}
