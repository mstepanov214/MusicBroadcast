namespace MusicBroadcast.Parser;

internal class LastfmParseResult
{
    public required IEnumerable<string> Urls { get; set; }

    public int PagesTotal { get; set; }
}
