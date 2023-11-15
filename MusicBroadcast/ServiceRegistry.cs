
using CommandLine;

using Microsoft.Extensions.DependencyInjection;

using MusicBroadcast.Converter;
using MusicBroadcast.Parser;

namespace MusicBroadcast;

static class ServiceRegistry
{
    public static IServiceProvider Provider { get; }

    static ServiceRegistry()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton(ParseStartupOptions());
        serviceCollection.AddSingleton<IBroadcastConfig>(BroadcastConfig.FromYaml("config.yaml"));
        serviceCollection.AddScoped<IParser<string[]>, LastfmParser>();
        serviceCollection.AddSingleton<IAudioSourceProvider, RandomAudioProvider>();
        serviceCollection.AddSingleton<IConverter, FFmpegAudioConverter>();
        serviceCollection.AddSingleton<Broadcast>();

        Provider = serviceCollection.BuildServiceProvider();
    }

    private static StartupOptions ParseStartupOptions()
    {
        var args = Environment.GetCommandLineArgs().Skip(1);
        return CommandLine.Parser.Default
            .ParseArguments<StartupOptions>(args)
            .WithNotParsed(_ => System.Environment.Exit(1))
            .Value;
    }
}