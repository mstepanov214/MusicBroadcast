using Microsoft.Extensions.DependencyInjection;

using MusicBroadcast;

CancellationTokenSource source = new();

Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs args) =>
{
    source.Cancel();
    Thread.Sleep(750);
};

try
{
    var broadcast = ServiceRegistry.Provider.GetRequiredService<Broadcast>();

    broadcast.Start(source.Token);
}
catch (OperationCanceledException)
{

}
finally
{
    source.Dispose();
}
