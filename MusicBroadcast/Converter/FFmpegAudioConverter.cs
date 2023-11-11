using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace MusicBroadcast.Converter
{
    internal class FFmpegAudioConverter : IConverter
    {
        private readonly bool _displayProgress = true;

        public FFmpegAudioConverter()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.ElementAtOrDefault(1) == "--hide-progress")
            {
                _displayProgress = false;
            }
        }

        public async Task Convert(string input, string output, CancellationToken ct)
        {
            var conversion = FFmpeg.Conversions.New().AddParameter("-hide_banner").AddParameter("-re");

            if (File.Exists("bg.jpg"))
            {
                conversion
                    .AddParameter("-loop 1")
                    .AddParameter("-f image2")
                    .AddParameter("-i bg.jpg");
            }
            else
            {
                conversion
                    .AddParameter("-f lavfi")
                    .AddParameter("-i color=size=1920x1080:rate=1:color=black");
            }

            conversion
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
                .AddParameter("-framerate 1")
                .AddParameter("-g 2")
                .AddParameter("-pix_fmt yuvj420p")
                .AddParameter("-tune stillimage")
                .AddParameter("-preset ultrafast")
                .AddParameter("-profile:v main")
                .AddParameter("-b:v 0")
                .AddParameter("-maxrate 4500k")
                .AddParameter("-bufsize 9000k")
                .AddParameter("-flvflags +no_duration_filesize+no_sequence_end")
                .AddParameter("-shortest")
                .AddParameter($"-f flv {output}");

            if (_displayProgress)
            {
                conversion.OnProgress += OnConversionProgress;
            }

            //conversion.OnDataReceived += (sender, args) =>
            //{
            //    Console.WriteLine(args.Data);
            //};

            try
            {
                await conversion.Start(ct);
            }
            catch (Xabe.FFmpeg.Exceptions.ConversionException e)
            {
                Console.WriteLine(e.InputParameters);
                throw new BroadcastException(e);
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
}
