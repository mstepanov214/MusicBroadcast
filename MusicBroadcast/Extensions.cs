namespace MusicBroadcast.Extensions;

static class Extensions
{
    public static bool IsAscii(this string str)
    {
        return str.All(c => c >= ' ' && c <= '~');
    }

    public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
    {
        if (!enumerable.Any())
        {
            throw new ArgumentException("Collection is empty");
        }
        return enumerable.ElementAt(new Random().Next(enumerable.Count()));
    }

    public static bool IsEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static bool IsNotEmpty(this string str)
    {
        return !IsEmpty(str);
    }
}