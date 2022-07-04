using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    public interface IService
    {
        /// <summary>
        /// Optional Priority attribute if multiple services of the same type are required, enables targeting a service for action.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Optional Priority to reorder registered managers based on their respective priority, reduces the risk of race conditions by prioritizing the order in which managers are evaluated.
        /// </summary>
        uint Priority { get; }

        void AddProvider<T>(T providerInstance) where T : IProvider;

        void RemoveProvider<T>(T providerInstance) where T : IProvider;

        /// <summary>
        /// The initialize function is used to setup the service once created.
        /// This method is called once all services have been registered in the Mixed Reality Toolkit.
        /// </summary>
        /// <remarks>This will run both in edit mode and in play mode. Gate code behind `Application.isPlaying` if it should only run in one or the other.</remarks>
        void Initialize();

        /// <summary>
        /// Optional Destroy function to perform cleanup of the service before the Mixed Reality Toolkit is destroyed.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Optional Reset function to perform that will Reset the service, for example, whenever there is a profile change.
        /// </summary>
        void Reset();

        /// <summary>
        /// Optional Enable function to enable / re-enable the service.
        /// </summary>
        void Enable();

        /// <summary>
        /// Optional Disable function to pause the service.
        /// </summary>
        void Disable();
    }
}
