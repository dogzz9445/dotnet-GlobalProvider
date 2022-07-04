using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    [ServiceAttribute(name: "BaseService", priority: 10)]
    public class BaseService : Notifiable, IService, IServiceState
    {
        public IProvider ProviderInstance { get; set; }

        protected ILoggerService _loggerService;
        public ILoggerService LoggerService
        {
            get
            {
                if (ProviderInstance != null)
                {
                    _loggerService ??= ProviderInstance.GetService<ILoggerService>();
                }
                return _loggerService;
            }
        }

        public BaseService()
        { }

        public string Name { get; }

        public uint Priority { get; }

        public bool IsInitialized { get; protected set; }

        public bool IsEnabled { get; protected set; }

        public bool IsMarkedDestroyed { get; protected set; }

        public virtual void Initialize()
        {
            IsInitialized = true;
            IsMarkedDestroyed = false;
        }

        public virtual void Destroy()
        {
            IsMarkedDestroyed = true;
            IsInitialized = false;
        }

        public virtual void Disable()
        {
            IsEnabled = false;
        }

        public virtual void Enable()
        {
            IsEnabled = true;
        }

        public virtual void Reset()
        {
        }

        public virtual void AddProvider<T>(T providerInstance) where T : IProvider
        {
            providerInstance.AddListener(Notify);
            ProviderInstance = providerInstance;
        }

        public virtual void RemoveProvider<T>(T providerInstance) where T : IProvider
        {
            ProviderInstance = null;
            providerInstance.AddListener(Notify);
        }
    }
}
