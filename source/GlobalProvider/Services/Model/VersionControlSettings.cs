using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if UNITY_ENGINE
using Newtonsoft.Json;
using JProperty = Newtonsoft.Json.JsonPropertyAttribute;
#else
using System.Text.Json;
using System.Text.Json.Serialization;
using JProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JIgnore = System.Text.Json.Serialization.JsonIgnoreAttribute;
#endif

namespace Mini.GlobalProvider
{
    public class VersionControlSettings : Notifiable
    {
#region 속성
        [JProperty("useVersionControl")]
        private bool? _useVersionControl;
        [JProperty("protocol")]
        private string? _protocol;
        [JProperty("address")]
        private string? _address;
        [JProperty("port")]
        private int? _port;
        [JProperty("apiContentRoot")]
        private string? _apiContentRoot;
        [JProperty("localFileRoot")]
        private string? _localFileRoot;
        [JProperty("username")]
        private string? _username;
        [JProperty("password")]
        private string? _password;

        [JIgnore]
        public bool? UseVersionControl { get => _useVersionControl; set => SetProperty(ref _useVersionControl, value); }
        [JIgnore]
        public string? LocalFileRoot { get => _localFileRoot; set => SetProperty(ref _localFileRoot, value); }
        [JIgnore]
        public string? Protocol { get => _protocol; set => SetProperty(ref _protocol, value); }
        [JIgnore]
        public string? Address { get => _address; set => SetProperty(ref _address, value); }
        [JIgnore]
        public int? Port { get => _port; set => SetProperty(ref _port, value); }
        [JIgnore]
        public string? ApiContentRoot { get => _apiContentRoot; set => SetProperty(ref _apiContentRoot, value); }
        [JIgnore]
        public string? Username { get => _username; set => SetProperty(ref _username, value); }
        [JIgnore]
        public string? Password { get => _password; set => SetProperty(ref _password, value); }
#endregion
        public VersionControlSettings() : this(null) { }

        public VersionControlSettings(
            bool? useContentDelivery = null,
            string? protocol = null,
            string? address = null,
            int? port = null,
            string? apiContentRoot = null,
            string? localFileRoot = null,
            string? username = null,
            string? password = null)
        {
            _useVersionControl = useContentDelivery ?? false;
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
