namespace MusicBroadcast.Parser;

public class LastfmParseResult
{
    public required IEnumerable<string> Urls { get; set; }

    public int PagesTotal { get; set; }
}
