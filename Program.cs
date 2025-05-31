using Muse.Src.Services;
using Muse.Src.Clients;
using Muse.Src.Handlers;
using Muse.Src.Entities;

static class Program
{
    public static async Task Main(string[] args)
    {
        var fileHandler = new FileHandler();
        LogHandler logger = new LogHandler(true, true, fileHandler);

        // Log inicial da execução
        var startLog = new LoggerInfo
        {
            Caller = "Program/Main",
            Message = "Application started"
        };
        logger.Info(startLog);

        string apiKey = new ApiKeyHandler(logger).GetApiKey();
        string? playlistUrl;
        using var client = new HttpClient();
        var downloader = new MusicDownloadService(logger, fileHandler);
        var youtubeApi = new YoutubeApiClient(client, apiKey, downloader, logger);

        do
        {
            var inputLog = new LoggerInfo
            {
                Caller = "Program/Main",
                Message = "Requesting YouTube URL from user"
            };
            logger.Info(inputLog);

            Console.Write("Enter the YouTube URL: ");
            playlistUrl = Console.ReadLine();

        } while (string.IsNullOrWhiteSpace(playlistUrl));

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
