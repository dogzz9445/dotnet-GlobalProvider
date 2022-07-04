using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    public interface ILogOutputReceiver
    {
        bool ParsesErrors { get; }

        void AddOutput(string line);
        void Flush();
    }
}
