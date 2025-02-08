using System.Web;

namespace MusicBroadcast.Extensions;

public static class Extensions
{
    public static bool IsAscii(this string str)
    {
        return str.All(c => c >= ' ' && c <= '~');
    }

    public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
    {
        if (!enumerable.Any())
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }
        return enumerable.ElementAt(new Random().Next(enumerable.Count()));
    }

    public static Uri AddParameter(this Uri url, string paramName, string paramValue)
    {
        var uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query[paramName] = paramValue;
        uriBuilder.Query = query.ToString();

        return uriBuilder.Uri;
    }
}