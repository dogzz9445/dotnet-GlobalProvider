using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider
{
    public class VersionControlSettings : Notifiable
    {
        #region 속성
        [JsonProperty("useContentDelivery")]
        private bool? _useContentDelivery;
        [JsonProperty("protocol")]
        private string _protocol;
        [JsonProperty("address")]
        private string _address;
        [JsonProperty("port")]
        private int? _port;
        [JsonProperty("apiContentRoot")]
        private string _apiContentRoot;
        [JsonProperty("localFileRoot")]
        private string _localFileRoot;
        [JsonProperty("username")]
        private string _username;
        [JsonProperty("password")]
        private string _password;

        [JsonIgnore]
        public bool? UseContentDelivery { get => _useContentDelivery; set => SetProperty(ref _useContentDelivery, value); }
        [JsonIgnore]
        public string LocalFileRoot { get => _localFileRoot; set => SetProperty(ref _localFileRoot, value); }
        [JsonIgnore]
        public string Protocol { get => _protocol; set => SetProperty(ref _protocol, value); }
        [JsonIgnore]
        public string Address { get => _address; set => SetProperty(ref _address, value); }
        [JsonIgnore]
        public int? Port { get => _port; set => SetProperty(ref _port, value); }
        [JsonIgnore]
        public string ApiContentRoot { get => _apiContentRoot; set => SetProperty(ref _apiContentRoot, value); }
        [JsonIgnore]
        public string Username { get => _username; set => SetProperty(ref _username, value); }
        [JsonIgnore]
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        #endregion
        public VersionControlSettings() : this(null) { }

        public VersionControlSettings(
            bool? useContentDelivery = null,
            string protocol = null,
            string address = null,
            int? port = null,
            string apiContentRoot = null,
            string localFileRoot = null,
            string username = null,
            string password = null)
        {
            _useContentDelivery = useContentDelivery ?? false;
            _localFileRoot = localFileRoot ?? "Resources/media";
            _protocol = protocol ?? "http";
            _address = address ?? "192.168.0.222";
            _port = port ?? 8910;
            _apiContentRoot = apiContentRoot ?? "/media/";
            _username = username ?? "";
            _password = password ?? "";
        }
    }
}
