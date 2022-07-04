using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    public class ServiceRegistry
    {
        private static readonly Dictionary<Type, List<KeyValuePair<IService, IServiceRegistrar>>> _serviceRegistry = 
            new Dictionary<Type, List<KeyValuePair<IService, IServiceRegistrar>>>();

        private static readonly Dictionary<IServiceRegistrar, List<IService>> _allServicesByRegistrar =
            new Dictionary<IServiceRegistrar, List<IService>>();

        private static readonly List<IService> _allServices = new List<IService>();

        private static readonly Comparer<IService> ascendingOrderComparer =
            Comparer<IService>.Create((i1, i2) => i1.Priority.CompareTo(i2.Priority));

        public ServiceRegistry()
        { }

        /// <summary>
        /// Adds an <see cref="IService"/> instance to the registry.
        /// </summary>
        /// <typeparam name="T">The interface type of the service being added.</typeparam>
        /// <param name="serviceInstance">Instance of the service to add.</param>
        /// <param name="registrar">Instance of the registrar manages the service.</param>
        /// <returns>
        /// True if the service was successfully added, false otherwise.
        /// </returns>
        public static bool AddService<T>(T serviceInstance, IServiceRegistrar registrar) where T : IService
        {
            if (serviceInstance == null)
            {
                // Adding a null service instance is not supported.
                return false;
            }

            //if (serviceInstance is IMixedRealityDataProvider)
            //{
            //    // Data providers are generally not used by application code. Services that intend for clients to
            //    // directly communicate with their data providers will expose a GetDataProvider or similarly named
            //    // method.
            //    return false;
            //}

            Type interfaceType = typeof(T);
            T existingService;

            if (TryGetService<T>(out existingService, serviceInstance.Name))
            {
                return false;
            }

            // Ensure we have a place to put our newly registered service.
            if (!_serviceRegistry.ContainsKey(interfaceType))
            {
                _serviceRegistry.Add(interfaceType, new List<KeyValuePair<IService, IServiceRegistrar>>());
            }

            List<KeyValuePair<IService, IServiceRegistrar>> services = _serviceRegistry[interfaceType];
            services.Add(new KeyValuePair<IService, IServiceRegistrar>(serviceInstance, registrar));
            AddServiceToCache(serviceInstance, registrar);
            return true;
        }

        /// <summary>
        /// Removes an <see cref="IService"/> instance from the registry.
        /// </summary>
        /// <typeparam name="T">The interface type of the service being removed.</typeparam>
        /// <param name="serviceInstance">Instance of the service to remove.</param>
        /// <param name="registrar">Instance of the registrar manages the service.</param>
        /// <returns>
        /// True if the service was successfully removed, false otherwise.
        /// </returns>
        public static bool RemoveService<T>(T serviceInstance, IServiceRegistrar registrar) where T : IService
        {
            return RemoveServiceInternal(typeof(T), serviceInstance, registrar);
        }

        /// <summary>
        /// Removes an <see cref="IService"/> instance from the registry.
        /// </summary>
        /// <typeparam name="T">The interface type of the service being removed.</typeparam>
        /// <param name="serviceInstance">Instance of the service to remove.</param>
        /// <returns>
        /// True if the service was successfully removed, false otherwise.
        /// </returns>
        public static bool RemoveService<T>(T serviceInstance) where T : IService
        {
            T tempService;
            IServiceRegistrar registrar;

            if (!TryGetService<T>(out tempService, out registrar))
            {
                return false;
            }

            if (!object.ReferenceEquals(serviceInstance, tempService))
            {
                return false;
            }

            return RemoveServiceInternal(typeof(T), serviceInstance, registrar);
        }

        /// <summary>
        /// Removes an <see cref="IService"/> instance from the registry.
        /// </summary>
        /// <typeparam name="T">The interface type of the service being removed.</typeparam>
        /// <param name="name">The friendly name of the service to remove.</param>
        /// <returns>
        /// True if the service was successfully removed, false otherwise.
        /// </returns>
        public static bool RemoveService<T>(string name) where T : IService
        {
            T tempService;
            IServiceRegistrar registrar;

            if (!TryGetService<T>(out tempService, out registrar, name))
            {
                return false;
            }

            return RemoveServiceInternal(typeof(T), tempService, registrar);
        }

        /// <summary>
        /// Removes an <see cref="IService"/> instance from the registry.
        /// </summary>
        /// <param name="interfaceType">The interface type of the service being removed.</param>
        /// <param name="serviceInstance">Instance of the service to remove.</param>
        /// <param name="registrar">Instance of the registrar manages the service.</param>
        /// <returns>
        /// True if the service was successfully removed, false otherwise.
        /// </returns>
        private static bool RemoveServiceInternal(
            Type interfaceType,
            IService serviceInstance,
            IServiceRegistrar registrar)
        {
            if (!_serviceRegistry.ContainsKey(interfaceType)) { return false; }

            List<KeyValuePair<IService, IServiceRegistrar>> services = _serviceRegistry[interfaceType];

            bool removed = services.Remove(new KeyValuePair<IService, IServiceRegistrar>(serviceInstance, registrar));

            if (services.Count == 0)
            {
                // If the last service was removed, the key can be removed.
                _serviceRegistry.Remove(interfaceType);
            }

            RemoveServiceFromCache(serviceInstance, registrar);

            return removed;
        }



        /// <summary>
        /// Adds the given service/registrar combination to the GetAllServices cache
        /// </summary>
        private static void AddServiceToCache(
            IService service,
            IServiceRegistrar registrar)
        {
            // Services are stored in ascending priority order - adding them to the
            // list requires that we re-enforce that order. This must happen
            // in both the allServices and allServicesByRegistrar data structures.
            _allServices.Add(service);
            _allServices.Sort(ascendingOrderComparer);

            if (!_allServicesByRegistrar.ContainsKey(registrar))
            {
                _allServicesByRegistrar.Add(registrar, new List<IService>());
            }

            _allServicesByRegistrar[registrar].Add(service);
            _allServicesByRegistrar[registrar].Sort(ascendingOrderComparer);
        }

        /// <summary>
        /// Removes the given service/registrar combination from the GetAllServices cache
        /// </summary>
        private static void RemoveServiceFromCache(
            IService service,
            IServiceRegistrar registrar)
        {
            // Removing from the sorted list keeps sort order, so re-sorting isn't necessary
            _allServices.Remove(service);
            if (_allServicesByRegistrar.ContainsKey(registrar))
            {
                _allServicesByRegistrar[registrar].Remove(service);
                if (_allServicesByRegistrar[registrar].Count == 0)
                {
                    _allServicesByRegistrar.Remove(registrar);
                }
            }
        }


        public static bool TryGetService<T>(
            out T serviceInstance,
            string name = null) where T : IService
        {
            return TryGetService<T>(
                out serviceInstance,
                out _,                  // The registrar out param is not used, it can be discarded.
                name);
        }

        public static bool TryGetService<T>(
            out T serviceInstance,
            out IServiceRegistrar registrar,
            string name = null) where T : IService
        {
            Type interfaceType = typeof(T);

            IService tempService;
            if (TryGetServiceInternal(interfaceType, out tempService, out registrar, name))
            {
                //Debug.Assert(tempService is T, "The service in the registry does not match the expected type.");
                serviceInstance = (T)tempService;
                return true;
            }

            serviceInstance = default(T);
            registrar = null;
            return false;
        }

        public static bool TryGetService<T>(Type interfaceType, 
            out IService serviceInstance,
            out IServiceRegistrar registrar, 
            string name = null)
        {
            if (!typeof(IService).IsAssignableFrom(interfaceType))
            {
                //Debug.LogWarning($"Cannot find type {interfaceType.Name} since it does not extend IService");
                serviceInstance = null;
                registrar = null;
                return false;
            }

            return TryGetServiceInternal(interfaceType, out serviceInstance, out registrar, name);
        }

        // private static readonly ProfilerMarker TryGetServiceInternalPerfMarker = new ProfilerMarker("[MRTK] MixedRealityServiceRegistry.TryGetServiceInternal");

        private static bool TryGetServiceInternal(Type interfaceType,
            out IService serviceInstance,
            out IServiceRegistrar registrar,
            string name = null)
        {
            //using (TryGetServiceInternalPerfMarker.Auto())
            //{
            //}
            // Assume failed and return null unless proven otherwise
            serviceInstance = null;
            registrar = null;

            // If there is an entry for the interface key provided, search that small list first
            if (_serviceRegistry.ContainsKey(interfaceType))
            {
                if (FindEntry(_serviceRegistry[interfaceType], interfaceType, name, out serviceInstance, out registrar))
                {
                    return true;
                }
            }

            // Either there is no entry for the interface type, or it was not placed in that list. 
            // Services can have multiple supported interfaces thus they may match the requested query but be placed in a different registry bin
            // Thus, search all bins until a match is found
            foreach (var list in _serviceRegistry.Values)
            {
                if (FindEntry(list, interfaceType, name, out serviceInstance, out registrar))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool FindEntry(List<KeyValuePair<IService, IServiceRegistrar>> serviceList,
            Type interfaceType,
            string name,
            out IService serviceInstance,
            out IServiceRegistrar registrar)
        {
            //using (FindEntryPerfMarker.Auto())
            //{
            //}
            // Assume failed and return null unless proven otherwise
            serviceInstance = null;
            registrar = null;

            for (int i = 0; i < serviceList.Count; ++i)
            {
                var svc = serviceList[i].Key;
                if ((string.IsNullOrEmpty(name) || svc.Name == name) && interfaceType.IsAssignableFrom(svc.GetType()))
                {
                    serviceInstance = svc;
                    registrar = serviceList[i].Value;

                    return true;
                }
            }

            return false;
        }

        public static void ClearAllServices()
        {
            if (_serviceRegistry != null)
            {
                _serviceRegistry.Clear();
                _allServices.Clear();
                _allServicesByRegistrar.Clear();
            }
        }

    }
}
