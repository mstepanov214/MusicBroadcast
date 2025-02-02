using CommandLine;

namespace MusicBroadcast;

public class StartupOptions
{
    [Option(
        "hide-progress",
        Default = false,
        HelpText = "Hide conversion progress display"
     )]
    public bool HideProgress { get; init; }
}
