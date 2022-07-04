using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    public interface IServiceState
    {
        bool IsInitialized { get; }

        bool IsEnabled { get; }

        bool IsMarkedDestroyed { get; }
    }
}
