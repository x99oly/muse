using Muse.Src.Services;
using Muse.Src.Clients;
using Muse.Src.Handlers;
using Muse.Src.Entities;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

static class Program
{
    public static async Task Main(string[] args)
    {
        var fileHandler = new FileHandler();
        LogHandler logger = new LogHandler(true, false, fileHandler);

        // Log inicial da execução
        var startLog = new LoggerInfo
        {
            Caller = "Program/Main",
            Message = "Application started"
        };
        logger.Info(startLog);

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

        await rootCommand.InvokeAsync(args);
    }

    public static async Task DownloadVideo(LogHandler logger, YoutubeApiClient youtubeApi, string playlistUrl)
    {
        try
        {
            var startDownloadLog = new LoggerInfo
            {
                Caller = "Program/Main",
                Message = $"Starting music download for URL: {playlistUrl}"
            };
            logger.Info(startDownloadLog);

            await youtubeApi.GetAndSaveMusic(playlistUrl);

            var successLog = new LoggerInfo
            {
                Caller = "Program/Main",
                Message = "Music download finished successfully"
            };
            logger.Info(successLog);
        }
        catch (Exception e)
        {
            var errorLog = new LoggerInfo
            {
                Caller = "Program/Main",
                Message = $"Unexpected error: {e.Message}\n{e.StackTrace}"
            };
            logger.Error(errorLog);
        }
    }
}
