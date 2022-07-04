using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    public interface IConfigurationService : IService
    {
        public string FullFileName { get; }
        public abstract void Load(string fullFileName);
        public abstract void Save(string fullFileName);
    }
}
