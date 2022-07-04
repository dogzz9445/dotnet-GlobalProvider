using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    public interface ILoggerService : IService
    {
        event NotifiedEventHandler Notified;
        abstract void Log(string log);
        abstract void LogInfo(string log);
        abstract void LogError(string log);
        abstract void LogStartTimer(string log);
        abstract void LogElapseTimer(string log);
        abstract void LogStopTimer(string log);
    }
}
