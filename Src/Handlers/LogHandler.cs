using Microsoft.Extensions.Logging;
using Muse.Src.Interfaces;
using Muse.Src.Enums;
using Muse.Src.Entities;

namespace Muse.Src.Handlers
{
    public class LogHandler: ILog
    {
        private readonly FileHandler _fh;
        private ILogger _logger;
        private ILoggerFactory _loggerFactory;
        private bool _isToPrintLog;
        private bool _isVerbose;
        private Dictionary<int, int> _logs = new Dictionary<int, int>();
        private string _logDirectory = "logs";
        private int _logIdLength = 9;

        public LogHandler(bool isVerbose, bool isToPrintLog, FileHandler fh)
        {
            _fh = fh;
            _isVerbose = isVerbose;
            _isToPrintLog = isToPrintLog;

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                if (_isVerbose)
                    builder.SetMinimumLevel(LogLevel.Debug);
                else
                    builder.SetMinimumLevel(LogLevel.Information);
            });

            _logger = _loggerFactory.CreateLogger<LogHandler>();

            Info(new LoggerInfo
            {
                Caller = "LogHandler",
                Message = "LogHandler created with success!"
            });
        }

        public void Info(ILoggerInfo log)
        {
            log.SetLevel(LogLevelEnum.Info);
            Log(log);
        }

        public void Warn(ILoggerInfo log)
        {
            log.SetLevel(LogLevelEnum.Warn);
            Log(log);
        }

        public void Error(ILoggerInfo log)
        {
            log.SetLevel(LogLevelEnum.Error);
            Log(log);
        }

        public void Debug(ILoggerInfo log)
        {
            log.SetLevel(LogLevelEnum.Debug);
            Log(log);
        }

        public void Critical(ILoggerInfo log)
        {
            log.SetLevel(LogLevelEnum.Critical);
            Log(log);
        }

        public void Trace(ILoggerInfo log)
        {
            log.SetLevel(LogLevelEnum.Trace);
            Log(log);
        }
        private void Log(ILoggerInfo log)
        {
            log.SetId(GetLogId());
            string content = log.GetLog();
            if (_isToPrintLog)
                _logger.LogInformation(content);

            _fh.SaveFile(Path.Combine(_logDirectory,"logs.txt"), content, false);
        }

        private int GetLogId()
        {
            int newId = 0;
            Identifier identifier = new Identifier(this);
            while (true)
            {
                newId = identifier.GetRandomIntId(_logIdLength);
                if (!_logs.ContainsKey(newId))
                {
                    _logs[newId] = newId;
                    break;
                }
            }
            return newId;
        }

    }
}