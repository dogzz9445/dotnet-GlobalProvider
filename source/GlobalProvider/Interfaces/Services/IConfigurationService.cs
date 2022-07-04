using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace Mini.GlobalProvider
{
    public interface IConfigurationService : IService
    {
        string? FullFileName { get; }
        abstract void Load(string fullFileName);
        abstract void Save(string fullFileName);
    }
}
