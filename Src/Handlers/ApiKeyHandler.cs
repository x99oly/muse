using Muse.Src.Interfaces;
using Muse.Src.Entities;
using System;

namespace Muse.Src.Handlers
{
    public class ApiKeyHandler
    {
        private readonly ILog _logger;

        public ApiKeyHandler(ILog logger)
        {
            _logger = logger;
        }

        public string GetApiKey()
        {
            string? apiKey = Environment.GetEnvironmentVariable("YOUTUBE_API_KEY");
            if (apiKey is not null) return apiKey;

            _logger.Info("Could not find YouTube API key, please enter it (or press 0[zero] to exit):");

            TryGetAndSaveApiKey();

            return GetApiKey();
        }

        public void TryGetAndSaveApiKey()
        {
            string? key = null;
            do
            {
                key = Console.ReadLine();
                if (key == "0")
                {
                    _logger.Info("Ending program...");
                    Environment.Exit(0);
                }
            } while (string.IsNullOrWhiteSpace(key));

            Environment.SetEnvironmentVariable("YOUTUBE_API_KEY", key);
        }
    }
}
