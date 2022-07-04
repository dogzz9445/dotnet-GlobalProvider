using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

#nullable enable
namespace Mini.GlobalProvider
{
    public class GlobalProvider<T> : Provider<T>, IGlobalProvider
        where T : class, INotified, new()
    {
        protected static Provider<T> _instance = new Provider<T>();
        public static Provider<T> Instance => _instance;

        protected GlobalProvider() {}

        public static dynamic Context(object? context = null)
        {
            return Instance.Context(context);
        }

        public static new IProvider RegisterService<TService>(TService serviceInstance) where TService : IService
        {
            Instance.RegisterService(serviceInstance);
            return Instance;
        }

        public static new bool UnregisterService<TService>(string name = null) where TService : IService
        {
            // TODO:

            return Instance.UnregisterService<TService>(name);
        }

        public static new bool UnregisterService<TService>(TService serviceInstance) where TService : IService
        {
            serviceInstance.RemoveProvider((IProvider)Instance);
            return Instance.UnregisterService(serviceInstance);
        }

        public static new bool IsServiceRegistered<TService>(string name = null) where TService : IService
        {
            return Instance.IsServiceRegistered<TService>(name);
        }

        public static new TService GetService<TService>(string name = null, bool showLogs = true) where TService : IService
        {
            return Instance.GetService<TService>(name, showLogs);
        }
        public static new IReadOnlyList<TService> GetServices<TService>(string name = null) where TService : IService
        {
            return Instance.GetServices<TService>(name);
        }

        public static new IProvider AddListener(NotifiedEventHandler handler)
        {
            Instance.AddListener(handler);
            return Instance;
        }

        public static new IProvider RemoveListener(NotifiedEventHandler handler)
        {
            Instance.RemoveListener(handler);
            return Instance;
        }

        public static new IProvider RemoveAllListener()
        {
            Instance.RemoveAllListener();
            return Instance;
        }
    }
}
