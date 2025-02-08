using Microsoft.Extensions.DependencyInjection;

using MusicBroadcast.Broadcast;
using MusicBroadcast.Converter;
using MusicBroadcast.Parser;

namespace MusicBroadcast;

public static class ServiceRegistry
{
    public static IServiceProvider Provider { get; }

    static ServiceRegistry()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IBroadcastConfig>(BroadcastConfig.FromYaml(FilePaths.Config));
        serviceCollection.AddScoped<IParser<LastfmParseResult>, LastfmParser>();
        serviceCollection.AddSingleton<IAudioProvider, RandomAudioProvider>();
        serviceCollection.AddSingleton<IConverter, FFmpegAudioConverter>();
        serviceCollection.AddSingleton<AudioBroadcast>();

        Provider = serviceCollection.BuildServiceProvider();
    }
}