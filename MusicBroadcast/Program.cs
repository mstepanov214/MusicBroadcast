using Microsoft.Extensions.DependencyInjection;

using MusicBroadcast;
using MusicBroadcast.Broadcast;

CancellationTokenSource source = new();

Console.CursorVisible = false;

Console.CancelKeyPress += (sender, args) =>
{
    source.Cancel();
    Thread.Sleep(750);
};

try
{
    var broadcast = ServiceRegistry.Provider.GetRequiredService<AudioBroadcast>();

    await broadcast.Start(source.Token);
}
catch (OperationCanceledException)
{

}
finally
{
    source.Dispose();
}
