using Muse.Src.Services;
using Muse.Src.Clients;
using Muse.Src.Handlers;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

static class Program
{
    public static async Task Main(string[] args)
    {
        var fileHandler = new FileHandler();
        LogHandler logger = new LogHandler(false, true, fileHandler);

        logger.Info("Application started");
        logger.Critical("Só testando isso");

        string apiKey = new ApiKeyHandler(logger).GetApiKey();
        using var client = new HttpClient();
        var downloader = new MusicDownloadService(logger, fileHandler);
        var youtubeApi = new YoutubeApiClient(client, apiKey, downloader, logger);

        Argument linkArg = new Argument<string>(
            name: "link",
            description: "The following url of the video or playlist."
        );

        Command downloadCommand = new Command("d", "Try download the video or playlist.")
        {
            linkArg
        };

        downloadCommand.Handler = CommandHandler.Create<string>(async (link) =>
        {
            await DownloadVideo(logger, youtubeApi, link);
        });

        RootCommand rootCommand = new("Muse CLI");
        rootCommand.AddCommand(downloadCommand);

        logger.Info("CLI commands configured");

        await rootCommand.InvokeAsync(args);
    }

    public static async Task DownloadVideo(LogHandler logger, YoutubeApiClient youtubeApi, string playlistUrl)
    {
        try
        {
            logger.Info($"Starting music download for URL: {playlistUrl}");

            await youtubeApi.GetAndSaveMusic(playlistUrl);

            logger.Info("Music download finished successfully");
        }
        catch (Exception e)
        {
            logger.Error($"Unexpected error: {e.Message}\n{e.StackTrace}", e);
        }
    }
}
