using Microsoft.Extensions.DependencyInjection;

using MusicBroadcast.Converter;
using MusicBroadcast.Parser;
using MusicBroadcast.Youtube;

namespace MusicBroadcast
{
    static class ServiceRegistry
    {
        public static IServiceProvider Provider { get; }

        static ServiceRegistry()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IBroadcastConfig>(BroadcastConfig.FromYaml("config.yaml"));
            serviceCollection.AddScoped<IParser<string[]>, LastfmParser>();
            serviceCollection.AddSingleton<IConverter, FFmpegAudioConverter>();
            serviceCollection.AddSingleton<IYoutubeDataProvider, YoutubeDataProvider>();
            serviceCollection.AddSingleton<Broadcast>();

            Provider = serviceCollection.BuildServiceProvider();
        }
    }
}