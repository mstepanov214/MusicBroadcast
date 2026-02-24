using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;

using MusicBroadcast.Extensions;

using RandomUserAgent;

namespace MusicBroadcast.Parser;

public class LastfmParser : IParser<LastfmParseResult>
{
    private readonly IBrowsingContext _browsingContext;

    public LastfmParser()
    {
        var requester = new DefaultHttpRequester(userAgent: RandomUa.RandomUserAgent);
        requester.Headers["Accept-Language"] = "en-US,en;q=0.5";
        requester.Headers["Accept-Encoding"] = "gzip, deflate, br";
        requester.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8";
        requester.Headers["Referer"] = "https://www.last.fm";

        var browsingConfiguration = Configuration.Default
            .With(requester)
            .WithDefaultLoader(new() { IsResourceLoadingEnabled = true });

        _browsingContext = BrowsingContext.New(browsingConfiguration);
    }

    public async Task<LastfmParseResult> Parse(string address)
    {
        try
        {
            var result = await ParseLastfmTag(address);

            return result;
        }
        catch (Exception ex)
        {
            throw new ParserException(ex);
        }
    }

    private async Task<LastfmParseResult> ParseLastfmTag(string address)
    {
        var document = await _browsingContext.OpenAsync(address);

        await document.WaitForReadyAsync();

        CheckResponseStatusCode(document);

        int pagesTotal = GetTotalPagesNumber(document);

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
        return new LastfmParseResult()
        {
            Urls = youtubeUrls,
            PagesTotal = pagesTotal
        };
    }

    private static int GetTotalPagesNumber(IDocument document)
    {
        var paginationButtons = document.QuerySelectorAll(".pagination-page");
        int count = paginationButtons.Length;

        return count == 0
            ? 1
            : int.Parse(paginationButtons[count - 1].Text().Trim());
    }

    private static void CheckResponseStatusCode(IDocument document)
    {
        var statusCode = document.StatusCode;

        if (!new HttpResponseMessage(document.StatusCode).IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Unsuccessful response status code: {statusCode}");
        }
    }
}
