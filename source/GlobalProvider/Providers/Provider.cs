using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace Mini.GlobalProvider
{
    public class Provider<T> : Notifiable, IProvider
            where T : class, INotified, new()
    {
        public Provider()
        {
            ContextType = typeof(T);
            Notified += Provider_Notified;
        }

        private void Provider_Notified(object? sender, NotifiedEventArgs e)
        {
            GetService<ILoggerService>()?.Log($"Provider::Provider_Notified");
        }

        protected dynamic? _context;

        public dynamic Context(object? context = null)
        {
            if (context != null)
            {
                SetObservablePropertyWithoutNotify(ref _context, context);
            }
            else if (_context == null)
            {
                SetObservablePropertyWithoutNotify(ref _context, new T());
            }
            return _context!;
        }

        //public T Context<T>(T? context = null)
        //    where T : class, INotified, new()
        //{
        //    ContextType = typeof(T);
        //    if (context != null)
        //    {
        //        SetObservablePropertyWithoutNotify(ref _context, context);
        //    }
        //    else if (_context == null)
        //    {
        //        SetObservablePropertyWithoutNotify(ref _context, new T());
        //    }
        //    return _context!;
        //}

        public Type ContextType { get; protected set; }

        public void AddListener(NotifiedEventHandler handler)
        {
            Notified += handler;
        }

        public void RemoveListener(NotifiedEventHandler handler)
        {
            Notified -= handler;
        }

        public void RemoveAllListener()
        {
            Notified -= null;
        }


        //protected readonly Dictionary<Type, Func<object>> _services = new Dictionary<Type, Func<object>>();
        protected readonly Dictionary<Type, WeakReference<IService>> _services = new Dictionary<Type, WeakReference<IService>>();

        // TODO:
        // 확인 필요
        public TService? GetService<TService>() where TService : IService
        {
            Type serviceType = typeof(TService);

            if (_services.TryGetValue(serviceType, out WeakReference<IService> weakService))
            {
                IService serviceReference;
                if (weakService.TryGetTarget(out serviceReference))
                {
                    return (TService)serviceReference;
                }

                _services.Remove(serviceType);
            }

            TService service;
            if (!ServiceRegistry.TryGetService(out service))
            {
                return default(TService);
            }

            _services.Add(serviceType, new WeakReference<IService>(service, false));
            return service;
        }

        /// <inheritdoc />
        public TService? GetService<TService>(string? name = null, bool showLogs = true) where TService : IService
        {
            TService serviceInstance = FindService<TService>(name);

            if (showLogs && (serviceInstance == null))
            {
                //Debug.LogError($"Failed to get the requested service of type {typeof(TService)}.");
            }

            return serviceInstance;
        }

        /// <inheritdoc />
        public IReadOnlyList<TServices> GetServices<TServices>(string? name = null) where TServices : IService
        {
            Type interfaceType = typeof(TServices);
            List<TServices> matchingServices = new List<TServices>();

            IService serviceReference;
            foreach (WeakReference<IService> service in _services.Values)
            {
                if (!service.TryGetTarget(out serviceReference)) { continue; }
                if (!interfaceType.IsAssignableFrom(service.GetType())) { continue; }

                // If a name has been provided and if it matches the services's name, add the service.
                if (!string.IsNullOrWhiteSpace(name) && string.Equals(serviceReference.Name, name))
                {
                    matchingServices.Add((TServices)serviceReference);
                }
                // If no name has been specified, always add the service.
                else
                {
                    matchingServices.Add((TServices)serviceReference);
                }
            }

            return matchingServices;
        }

        /// <inheritdoc />
        public bool IsServiceRegistered<TService>(string? name = null) where TService : IService
        {
            return (GetService<TService>(name) != null);
        }

        /// <inheritdoc />
        public IProvider RegisterService<TService>(TService serviceInstance)
            where TService : IService
        {
            Type interfaceType = typeof(TService);

            if (_services.ContainsKey(interfaceType))
            {
                //Debug.LogError($"Failed to register {serviceInstance} service. There is already a registered service implementing {interfaceType}");
                return this;
            }

            bool registered = ServiceRegistry.AddService(serviceInstance, this);
            if (registered)
            {
                // Service Provider
                serviceInstance.AddProvider(this);
                _services.Add(interfaceType, new WeakReference<IService>(serviceInstance));
                serviceInstance.Initialize();
            }

            return this;
        }

        /// <inheritdoc />
        public bool UnregisterService<TService>(string? name = null) where TService : IService
        {
            TService serviceInstance = FindService<TService>(name);

            if (serviceInstance == null) { return false; }

            // Service Provider
            serviceInstance.RemoveProvider(this);

            return UnregisterService<TService>(serviceInstance);
        }

        /// <inheritdoc />
        public bool UnregisterService<TService>(TService? serviceInstance) where TService : IService
        {
            if (serviceInstance == null) { return false; }

            Type interfaceType = typeof(TService);
            if (!_services.ContainsKey(interfaceType)) { return false; }

            _services.Remove(interfaceType);

            return ServiceRegistry.RemoveService<TService>(serviceInstance, this);
        }

        /// <summary>
        /// Locates a service instance in the registry,
        /// </summary>
        /// <typeparam name="T">The interface type of the service to locate.</typeparam>
        /// <param name="name">The name of the desired service.</param>
        /// <returns>Instance of the interface type, or null if not found.</returns>
        private TService FindService<TService>(string? name = null) where TService : IService
        {
            Type interfaceType = typeof(TService);
            WeakReference<IService> serviceInstance;

            if (!_services.TryGetValue(interfaceType, out serviceInstance)) { return default; }

            IService serviceReference;
            if (serviceInstance.TryGetTarget(out serviceReference))
            {
                return (TService)serviceReference;
            }
            return default;
        }

        //public Producer<T> Register<TService, TImplement>() where TImplement : TService
        //{
        //    _services.Add(typeof(TService), () => this.Resolve(typeof(TImplement)));
        //    return this;
        //}

        //public Producer<T> Register<TService, TImplement>(TImplement implement) where TImplement : TService
        //{
        //    _services.Add(typeof(TService), () => this.Resolve(implement));
        //    return this;
        //} 

        //public Producer<T> Register<TService>(Func<TService> instanceCreator)
        //{
        //    _services.Add(typeof(TService), () => instanceCreator);
        //    return this;
        //}

        //public object Resolve(Type serviceType)
        //{
        //    Func<object> creator;
        //    if (_services.TryGetValue(serviceType, out creator)) return creator();
        //    else if (!serviceType.IsAbstract) return this.CreateInstance(serviceType);
        //    else throw new InvalidOperationException("No registration for " + serviceType);
        //}

        //private object Resolve<TImplement>(TImplement Implement)
        //{
        //    Type serviceType = Implement.GetType();
        //    Func<object> creator;
        //    if (_services.TryGetValue(Implement.GetType(), out creator)) return creator();
        //    else if (!serviceType.IsAbstract) return this.CreateInstance(serviceType);
        //    else throw new InvalidOperationException("No registration for " + serviceType);
        //}

        //private object CreateInstance(Type implementationType)
        //{
        //    var ctor = implementationType.GetConstructors().Single();
        //    var parameterTypes = ctor.GetParameters().Select(p => p.ParameterType);
        //    var dependencies = parameterTypes.Select(t => this.Resolve(t)).ToArray();
        //    object instance = Activator.CreateInstance(implementationType, dependencies);

        //    return instance;
        //}

        public void Release()
        {
            _services.Clear();
        }
    }
}
