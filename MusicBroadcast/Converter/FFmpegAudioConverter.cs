using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace MusicBroadcast.Converter;

public class FFmpegAudioConverter : IConverter
{
    private readonly StartupOptions _startupOptions;
    private const int _maxRate = 6800;

    public FFmpegAudioConverter(StartupOptions startupOptions)
    {
        _startupOptions = startupOptions;
    }

    public async Task Convert(string input, string output, CancellationToken ct)
    {
        if (!File.Exists(FilePaths.BackgroundImage))
        {
            throw new FileNotFoundException("Background image file was not found", FilePaths.BackgroundImage);
        }

        var conversion = FFmpeg.Conversions.New()
            .AddParameter("-hide_banner")
            .AddParameter("-stream_loop -1")
            .AddParameter("-re")
            .AddParameter("-f image2")
            .AddParameter($"-i {FilePaths.BackgroundImage}")
            .AddParameter("-reconnect 1")
            .AddParameter("-reconnect_streamed 1")
            .AddParameter("-reconnect_delay_max 5")
            .AddParameter("-fflags +discardcorrupt+igndts")
            .AddParameter($"-i {input}")
            .AddParameter("-map 0:v")
            .AddParameter("-map 1:a")
            .AddParameter("-ar 44100")
            .AddParameter("-c:a aac")
            .AddParameter("-b:a 128k")
            .AddParameter("-c:v libx264")
            .AddParameter("-s:v 1920x1080")
            //.AddParameter("-framerate 30")
            .AddParameter("-r 30")
            .AddParameter("-g 60")
            //.AddParameter("-pix_fmt yuvj420p")
            .AddParameter("-pix_fmt yuv420p")
            .AddParameter("-tune stillimage")
            .AddParameter("-preset ultrafast")
            .AddParameter("-crf 23")
            .AddParameter("-profile:v main")
            .AddParameter("-b:v 2500k")
            .AddParameter($"-maxrate {_maxRate}k")
            .AddParameter($"-bufsize {_maxRate * 2}k")
            .AddParameter("-threads 6 -qscale 3")
            .AddParameter("-flvflags +no_duration_filesize+no_sequence_end")
            .AddParameter("-shortest")
            .AddParameter($"-f flv {output}");

        if (!_startupOptions.HideProgress)
        {
            conversion.OnProgress += OnConversionProgress;
        }

        try
        {
            await conversion.Start(ct);
        }
        catch (Xabe.FFmpeg.Exceptions.ConversionException e)
        {
            Console.WriteLine(e.InputParameters);
            throw new ConverterException(e);
        }
    }

    private static void OnConversionProgress(object sender, ConversionProgressEventArgs args)
    {
        ClearCurrentConsoleLine();
        Console.WriteLine($"[{args.Duration} / {args.TotalLength}] {args.Percent}%");
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }

    private static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
}
