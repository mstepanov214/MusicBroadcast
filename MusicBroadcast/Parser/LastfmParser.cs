using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;

using MusicBroadcast.Extensions;

using RandomUserAgent;

namespace MusicBroadcast.Parser;

internal class LastfmParser : IParser<string[]>
{
    private readonly IBrowsingContext _browsingContext;

    public LastfmParser()
    {
        var requester = new DefaultHttpRequester(userAgent: RandomUa.RandomUserAgent);
        requester.Headers["Accept-Language"] = "en-US,en;q=0.5";
        requester.Headers["Accept-Encoding"] = "gzip, deflate, br";
        requester.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8";
        requester.Headers["Referer"] = "https://www.last.fm";

        var loaderOptions = new LoaderOptions
        {
            IsResourceLoadingEnabled = true
        };
        _browsingContext = BrowsingContext.New(Configuration.Default.With(requester).WithDefaultLoader(loaderOptions));
    }

    public async Task<string[]> Parse(string address)
    {
        var document = await _browsingContext.OpenAsync(address);

        await document.WaitForReadyAsync();

        var rows = document.QuerySelectorAll("tr.chartlist-row");

        var youtubeUrls = new List<string>();

        foreach (var row in rows)
        {
            string? name = row.QuerySelector(".chartlist-name")?.Text()?.Trim();
            string? artist = row.QuerySelector(".chartlist-artist")?.Text()?.Trim();

            if (name?.IsAscii() == true && artist?.IsAscii() == true)
            {
                var link = row.QuerySelector<IHtmlAnchorElement>(".chartlist-play-button");

                if (link?.Href != null)
                {
                    youtubeUrls.Add(link.Href);
                }
            }
        }
        return youtubeUrls.ToArray();
    }
}
