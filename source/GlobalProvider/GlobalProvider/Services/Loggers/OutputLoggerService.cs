using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mini.GlobalProvider
{
    public class OutputLoggerService : BaseService, ILoggerService
    {
        private OutputLoggerService() { }

        public ILogOutputReceiver LogOutputReceiver { get; set; }

        public OutputLoggerService(ILogOutputReceiver logOutputReceiver)
        {
            LogOutputReceiver = logOutputReceiver;
        }

        public void Log(string log)
        {
            LogOutputReceiver.AddOutput($"Log: {log}");
        }

        public void LogElapseTimer(string log)
        {
            LogOutputReceiver.AddOutput($"Log: {log}");
        }

        public void LogError(string log)
        {
            LogOutputReceiver.AddOutput($"LogError: {log}");
        }

        public void LogInfo(string log)
        {
            LogOutputReceiver.AddOutput($"LogInfo:: {log}");
        }

        public void LogStartTimer(string log)
        {
            LogOutputReceiver.AddOutput($"LogInfo:: {log}");
        }

        public void LogStopTimer(string log)
        {
            LogOutputReceiver.AddOutput($"LogInfo:: {log}");
        }
    }
}
