using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace Mini.GlobalProvider
{
    public interface IGlobalServiceRegistrar<T> where T : class, INotified, new()
    {
        public abstract static Provider<T> RegisterService<TService>(TService serviceInstance) where TService : IService;
        public abstract static bool UnregisterService<TService>(string name = null) where TService : IService;
        public abstract static bool UnregisterService<TService>(TService serviceInstance) where TService : IService;
        public abstract static bool IsServiceRegistered<TService>(string name = null) where TService : IService;
        public abstract static TService GetService<TService>(string name = null, bool showLogs = true) where TService : IService;
        public abstract static IReadOnlyList<TService> GetServices<TService>(string name = null) where TService : IService;
    }

    public interface IGlobalProvider //<T> where T : class, INotified, new()
    {
        public abstract static dynamic Context(object? context = null);
        public abstract static IProvider AddListener(NotifiedEventHandler handler);
        public abstract static IProvider RemoveListener(NotifiedEventHandler handler);
        public abstract static IProvider RemoveAllListener();
        public abstract static IProvider RegisterService<TService>(TService serviceInstance) where TService : IService;
        public abstract static bool UnregisterService<TService>(string name = null) where TService : IService;
        public abstract static bool UnregisterService<TService>(TService serviceInstance) where TService : IService;
        public abstract static bool IsServiceRegistered<TService>(string name = null) where TService : IService;
        public abstract static TService GetService<TService>(string name = null, bool showLogs = true) where TService : IService;
        public abstract static IReadOnlyList<TService> GetServices<TService>(string name = null) where TService : IService;
    }
}
