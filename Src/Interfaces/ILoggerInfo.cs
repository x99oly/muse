using Muse.Src.Enums;

namespace Muse.Src.Interfaces
{
    public interface ILoggerInfo
    {
        public void SetId(int id);
        public string GetLog();
        public void SetLevel(LogLevelEnum level);
    }
}