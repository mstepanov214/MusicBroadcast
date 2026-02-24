using System.Web;

namespace MusicBroadcast.Extensions;

public static class Extensions
{
    extension(string str)
    {
        public bool IsAscii()
        {
            return str.All(c => c >= ' ' && c <= '~');
        }
    }

    extension<T>(IEnumerable<T> enumerable)
    {
        public T GetRandomElement()
        {
            int count = enumerable.Count();
            ArgumentOutOfRangeException.ThrowIfZero(count);
            return enumerable.ElementAt(new Random().Next(count));
        }
    }

    extension(Uri url)
    {
        public Uri AddParameter(string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }
    }

    extension(Console)
    {
        public static void ClearCurrentLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }

    extension(FileNotFoundException)
    {
        public static void ThrowIfNotExists(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"{path} : file not found", Path.GetFileName(path));
            }
        }
    }
}