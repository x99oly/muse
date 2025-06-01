using System;
using System.Runtime.CompilerServices;

namespace Muse.Src.Interfaces
{
    public interface ILog
    {
        void Trace(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0);
        void Debug(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0);
        void Info(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0);
        void Warn(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0);
        void Error(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0);
        void Error(string msg, Exception e, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0);
        void Critical(string msg, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0);
    }
}
