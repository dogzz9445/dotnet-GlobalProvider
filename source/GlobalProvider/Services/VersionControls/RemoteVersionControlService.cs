using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Mini.GlobalProvider.Utils;
using Mini.GlobalProvider.Extensions;

namespace Mini.GlobalProvider
{

    [ServiceAttribute(name: "RemoteVersionControlService", priority: 5)]
    public class RemoteVersionControlService : BaseService, IVersionControlService
    {
        private VersionControlSettings _settings;
        private bool _isCheckedVersion = false;
        private bool _isLocalFileLatestVersion = false;
        public string ApiURL { get; private set; }
        public string SvcCredentials { get; private set; }
        public string LocalFileRoot { get; private set; }

        private RemoteVersionControlService() { }
        public RemoteVersionControlService(VersionControlSettings settings)
        {
            _settings = settings;
            ApiURL = PathHelper.Combine($"{settings.Protocol}://{settings.Address}:{settings.Port}", settings.ApiContentRoot);
            SvcCredentials = GenerateSvcCredentials(settings.Username, settings.Password);
            LocalFileRoot = settings.LocalFileRoot;
        }

        public override void Initialize()
        {
            base.Initialize();

            LoggerService?.Log($"RemoteVersionControlService Initialized");
        }

        public string GenerateSvcCredentials(string username, string password)
        {
            return Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));
        }

        public async Task DownloadFileInternalAsync(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                LoggerService?.LogError($"RemoteVersionService:: Null Parameter");
                return;
            }

            var saveFullFileName = PathHelper.Combine(LocalFileRoot, filename);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(PathHelper.Combine(ApiURL, filename));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Basic " + SvcCredentials);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                await client.DownloadFileTaskAsync(client.BaseAddress, saveFullFileName);
            }
        }

        public void CheckVersion()
        {
            _isCheckedVersion = true;
            _isLocalFileLatestVersion = false;
        }

        public string LoadFromFile()
        {

            return null;
        }

        public void DownloadOrLoad<T>(string filename, out Dictionary<uint, T> map)
        {
            map = new Dictionary<uint, T>();
            if (!_isCheckedVersion)
            {
                CheckVersion();
            }
            if (!_isLocalFileLatestVersion)
            {
            }
            var task = DownloadFileInternalAsync(filename);
            task.Wait();
            var json = LoadFromFile();
            // CheckVersion
            // RequestAPIAsync();
        }

        public void Download<T>(string filename)
        {
            throw new NotImplementedException();
        }

        public void Load<T>(string filename, out Dictionary<uint, T> map)
        {
            throw new NotImplementedException();
        }
    }
}
