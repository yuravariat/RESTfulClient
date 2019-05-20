using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfulClient
{
    public interface ILogger
    {
        void WriteLog(string message, Exception ex, Level level, params object[] parameters);
    }
    public enum Level : short
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warn = 4,
        Error = 8,
        Fatal = 16
    }
}
