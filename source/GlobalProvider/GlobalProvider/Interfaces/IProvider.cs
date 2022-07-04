using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace Mini.GlobalProvider
{
    public interface IProvider : IServiceRegistrar
    {
        Type ContextType { get; }
        dynamic Context(object? context = null);
        public abstract void AddListener(NotifiedEventHandler handler);
        public abstract void RemoveListener(NotifiedEventHandler handler);
        public abstract void RemoveAllListener();
    }

}
