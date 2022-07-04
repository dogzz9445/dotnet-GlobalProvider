using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UNITY_ENGINE
using UnityEngine;
#endif

namespace Mini.GlobalProvider
{
    public class UnityLoggerService : BaseService, ILoggerService
    {
        public void Log(string log)
        {
#if UNITY_ENGINE
            Debug.Log(log);
#endif
        }

        public void LogElapseTimer(string log)
        {
            throw new NotImplementedException();
        }

        public void LogError(string log)
        {
#if UNITY_ENGINE
            Debug.LogError(log);
#endif
        }

        public void LogInfo(string log)
        {
#if UNITY_ENGINE
            Debug.LogInfo(log);
#endif
        }

        public void LogStartTimer(string log)
        {
            throw new NotImplementedException();
        }

        public void LogStopTimer(string log)
        {
            throw new NotImplementedException();
        }
    }
}
