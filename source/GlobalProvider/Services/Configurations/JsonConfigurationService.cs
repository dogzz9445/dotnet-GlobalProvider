using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Mini.GlobalProvider.Utils;

#nullable enable
namespace Mini.GlobalProvider
{
    [ServiceAttribute(name: "JsonConfigurationService", priority: 8)]
    public class JsonConfigurationService : BaseService, IConfigurationService
    {
        private string? _fullFileName;
        public string? FullFileName { get => _fullFileName; set => _fullFileName = value; }
        private bool _isAutoSave = true;

        public bool IsAutoSave { get => _isAutoSave; set => _isAutoSave = value; }

        public JsonConfigurationService(string? fileName = null) : base()
        {
            Notified += OnNotified;
            IsInitialized = false;
            IsEnabled = false;

            if (!string.IsNullOrEmpty(fileName))
            {
                FullFileName = fileName;
            }
        }
       
        public override void Initialize()
        {
            base.Initialize();

            LoggerService?.Log("JsonConfigurationService::Initialize: Disable...");
            Disable();
            Load();
            Save();
            Enable();
            LoggerService?.Log("JsonConfigurationService::Initialize: Enable...");
        }

        public void Load(string? fullFileName = null)
        {
            LoggerService?.Log("JsonConfigurationService::Load: Loading...");
            if (!string.IsNullOrEmpty(fullFileName))
            {
                _fullFileName = fullFileName;
            }
            ProviderInstance?.Context(JsonHelper.ReadFileOrDefault(ProviderInstance.ContextType, _fullFileName));
            LoggerService?.Log("JsonConfigurationService::Load: Loaded...");
        }

        public async void Save(string? fullFileName = null)
        {
            LoggerService?.Log("JsonConfigurationService::Save: Saving...");
            if (!string.IsNullOrEmpty(fullFileName))
            {
                _fullFileName = fullFileName;
            }
            await JsonHelper.WriteFileAsync(_fullFileName, ProviderInstance.Context(null));
            LoggerService?.Log("JsonConfigurationService::Save: Saved...");
        }

        public void OnNotified(object? sender, NotifiedEventArgs eventArgs)
        {
            if (!IsEnabled || !IsAutoSave)
            {
                return;
            }
            Save();
        }
    }
}
