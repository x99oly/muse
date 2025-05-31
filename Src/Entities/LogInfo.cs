using Muse.Src.Interfaces;
using Muse.Src.Enums;

namespace Muse.Src.Entities
{
    public class LoggerInfo : ILoggerInfo
    {
        public LogLevelEnum Level { get; set; } = LogLevelEnum.Info;
        private int Id = 0;
        public string? Caller { get; set; } = null;
        public int CallTime { get; set; } = 0;
        public string? Message { get; set; } = null;

        public void SetId(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Property 'id' must be greater than zero.");

            // if (Id != 0) 
            //     throw new InvalidOperationException("Id is already set.");

            Id = id;
        }

        public string GetLog()
        {
            if (Id == 0)
                throw new InvalidOperationException("Log not started correctly.");

            return $"{Level} - id: {Id}, timestamp: {(CallTime > 0 ? CallTime : DateTimeOffset.UtcNow.ToUnixTimeSeconds())}, caller: {Caller ?? "Unspecified"}" +
                   (Message != null ? $", message: {Message}" : "");
        }

        public void SetLevel(LogLevelEnum level) => Level = level;
    }
}
