using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceAttribute : Attribute
    {
        /// <summary>
        /// The friendly name for this service.
        /// </summary>
        public virtual string Name { get; }

        public virtual uint Priority { get; }

        ///// <summary>
        ///// Is a profile explicitly required?
        ///// </summary>
        //public virtual bool RequiresProfile { get; }

        ///// <summary>
        ///// The file path to the default profile asset relative to the package folder.
        ///// </summary>
        //public virtual string DefaultProfilePath { get; }

        ///// <summary>
        ///// The package where the default profile asset resides.
        ///// </summary>
        //public virtual string PackageFolder { get; }



        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceAttribute(
            string name = "",
            uint priority = 10)
        {
            Name = name;
            Priority = priority;
        }
    }
}
