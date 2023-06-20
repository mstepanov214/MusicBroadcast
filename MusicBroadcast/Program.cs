using MusicBroadcast;
using MusicBroadcast.Converter;
using MusicBroadcast.Parser;

CancellationTokenSource source = new();

Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs args) =>
{
    source.Cancel();
    Thread.Sleep(750);
};

try
{
    var config = BroadcastConfig.FromYaml("config.yaml");

    var broadcast = new Broadcast(
        config,
        new LastfmParser(),
        new FFmpegAudioConverter()
    );

    broadcast.Start(source.Token);
}
catch (OperationCanceledException)
{

}
finally
{
    source.Dispose();
}
