using Microsoft.Extensions.Configuration;

namespace MusicBroadcast;

internal class BroadcastConfig : IBroadcastConfig
{
    public required string OutputUrl { get; init; }

    public required string TracksUrl { get; init; }

    public required int PagesTotal { get; init; }

    public static BroadcastConfig FromYaml(string path)
    {
        IConfiguration configuration = new ConfigurationBuilder()
         .AddYamlFile(path)
         .Build();

        var broadcastConfig = configuration.Get<BroadcastConfig>((binderOptions) => binderOptions.ErrorOnUnknownConfiguration = true)!;

        foreach (var item in configuration.AsEnumerable())
        {
            if (string.IsNullOrEmpty(item.Value))
            {
                throw new ArgumentException("Config parameter is not specified", item.Key);
            }
        }
        return broadcastConfig;
    }
}
