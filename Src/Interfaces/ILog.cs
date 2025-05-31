namespace Muse.Src.Interfaces
{
    public interface ILog
    {
        void Trace(ILoggerInfo log);
        void Debug(ILoggerInfo log);
        void Info(ILoggerInfo log);
        void Warn(ILoggerInfo log);
        void Error(ILoggerInfo log);
        void Critical(ILoggerInfo log);
    }
}
