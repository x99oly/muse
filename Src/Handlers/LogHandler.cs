using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Muse.Src.Interfaces;
using Muse.Src.Enums;

namespace Muse.Src.Handlers
{
    public class LogHandler : ILog
    {
        private readonly FileHandler _fh;
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly bool _isToPrintLog;
        private readonly bool _isVerbose;
        private readonly Dictionary<int, int> _logs = new Dictionary<int, int>();
        private readonly string _logDirectory = "logs";
        private readonly int _logIdLength = 9;

        public LogHandler(bool isVerbose, bool isToPrintLog, FileHandler fh)
        {
            _fh = fh;
            _isVerbose = isVerbose;
            _isToPrintLog = isToPrintLog;

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(_isVerbose ? LogLevel.Debug : LogLevel.Information);
            });

            _logger = _loggerFactory.CreateLogger<LogHandler>();

            Info("LogHandler created with success!");
        }

        public void Info(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) => Log(LogLevelEnum.Info, message, null, file, member, line);

        public void Warn(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) => Log(LogLevelEnum.Warn, message, null, file, member, line);

        public void Error(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) => Log(LogLevelEnum.Error, message, null, file, member, line);

        public void Error(string message, Exception ex, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) => Log(LogLevelEnum.Error, message, ex, file, member, line);

        public void Critical(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) => Log(LogLevelEnum.Critical, message, null, file, member, line);

        public void Debug(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) => Log(LogLevelEnum.Debug, message, null, file, member, line);

        public void Trace(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0) => Log(LogLevelEnum.Trace, message, null, file, member, line);

        private void Log(LogLevelEnum level, string message, Exception? ex, string file, string member, int line)
        {
            int id = GetLogId();
            string className = Path.GetFileNameWithoutExtension(file);
            string caller = $"File: {file}, Class: {className}, Method: {member}, Line: {line}";
            
            if (ex != null)
            {
                caller += $", Exception: {ex.GetType().Name}";
                message += $" | Exception Message: {ex.Message} | StackTrace: {ex.StackTrace}";
            }
            string content = $"{level.ToString().PadRight(8)} - Id: {id}, Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}, {caller}, Message: {message}";

            if (_isToPrintLog)
            {
                string pContent = content;
                if (!_isVerbose)
                {
                    caller = $"Class: {className}, Method: {member}, Line: {line}";
                    pContent = $"{level.ToString().PadRight(8)} - Id: {id}, Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}, {caller}, Message: {message}";
                }
                    
                switch (level)
                    {
                        case LogLevelEnum.Info: _logger.LogInformation(pContent); break;
                        case LogLevelEnum.Warn: _logger.LogWarning(pContent); break;
                        case LogLevelEnum.Error: _logger.LogError(pContent); break;
                        case LogLevelEnum.Debug: _logger.LogDebug(pContent); break;
                        case LogLevelEnum.Critical: _logger.LogCritical(pContent); break;
                        case LogLevelEnum.Trace: _logger.LogTrace(pContent); break;
                    }
            }

            _fh.SaveFile(Path.Combine(_logDirectory, "logs.txt"), content, false);
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
