using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    public interface IVersionControlService : IService
    {
        //public string RemoteVersion();
        //public bool CheckVersion(string version);
        //public bool Donwload(string url);
        void DownloadOrLoad<T>(string filename, out Dictionary<uint, T> map);
    }
}
