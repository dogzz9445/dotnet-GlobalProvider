using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    public interface IVersionControlService : IService
    {
        void DownloadOrLoad<T>(string filename, out Dictionary<uint, T> map);
        void Download<T>(string filename);
        void Load<T>(string filename, out Dictionary<uint, T> map);
    }
}
