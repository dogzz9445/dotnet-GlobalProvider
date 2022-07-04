using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mini.GlobalProvider.Utils
{
    public static class JsonHelper
    {
        public static T ReadFileOrDefault<T>(string filename)
        {
            if (!File.Exists(filename))
            {
                return default(T);
            }
            var json = File.ReadAllText(filename);
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(json,
                new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        public static dynamic ReadFileOrDefault(Type type, string filename)
        {
            if (!File.Exists(filename))
            {
                return default;
            }
            var json = File.ReadAllText(filename);
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            return JsonConvert.DeserializeObject(json, type,
                new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        //public static T ReadFileOrDefault<T>(string filename) where T : NotifiableBase
        //{
        //    if (!File.Exists(filename))
        //    {
        //        return new T();
        //    }
        //    var json = File.ReadAllText(filename);
        //    if (string.IsNullOrEmpty(json))
        //    {
        //        return new T();
        //    }

        //    return JsonConvert.DeserializeObject<T>(json,
        //        new JsonSerializerSettings()
        //        {
        //            DefaultValueHandling = DefaultValueHandling.Populate,
        //            NullValueHandling = NullValueHandling.Ignore
        //        });
        //}

        public static async Task WriteFileAsync<T>(string fullFileName, T jsonObject)
        {
            if (jsonObject == null)
            {
                return;
            }
            var directoryName = Path.GetDirectoryName(fullFileName);
            if (!string.IsNullOrEmpty(directoryName))
            {
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
            }

            using (var file = File.CreateText(fullFileName))
            {
                using (var writer = new JsonTextWriter(file))
                {
                    string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented,
                        new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    await writer.WriteRawAsync(json);
                }
            }
        }
        //public class EmptyStringConverter : JsonConverter<string>
        //{
        //    public override bool HandleNull => true;

        //    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //        => reader.TokenType == JsonTokenType.Null ? "" : reader.GetString();

        //    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) =>
        //        writer.WriteStringValue(value ?? "");
        //}

        //public static T ReadFileOrDefault<T>(string filename) where T : new()
        //{
        //    if (!File.Exists(filename))
        //    {
        //        return new T();
        //    }

        //    var json = File.ReadAllText(filename);
        //    if (string.IsNullOrEmpty(json))
        //    {
        //        return new T();
        //    }

        //    return JsonSerializer.Deserialize<T>(json,
        //        new JsonSerializerOptions()
        //        {
        //            IgnoreNullValues = true
        //        });
        //}

        //public static async Task WriteFileAsync<T>(string fullFileName, T jsonObject)
        //{
        //    var directoryName = Path.GetDirectoryName(fullFileName);
        //    if (!string.IsNullOrEmpty(directoryName))
        //    {
        //        if (!Directory.Exists(directoryName))
        //        {
        //            Directory.CreateDirectory(directoryName);
        //        }
        //    }

        //    string json = JsonSerializer.Serialize(jsonObject,
        //        new JsonSerializerOptions()
        //        {
        //            WriteIndented = true,
        //            //IgnoreNullValues = true,
        //            NumberHandling = JsonNumberHandling.AllowReadingFromString,

        //        });
        //    await File.WriteAllTextAsync(fullFileName, json);

        //    //using (var file = File.CreateText(fullFileName))
        //    //{
        //    //    using (var writer = new Utf8JsonWriter(file.BaseStream))
        //    //    {
        //    //        string json = JsonSerializer.Serialize(jsonObject,
        //    //            new JsonSerializerOptions()
        //    //            {
        //    //                WriteIndented = true,
        //    //                IgnoreNullValues = true
        //    //            });
        //    //        writer.WriteStringValue(json);
        //    //        await writer.FlushAsync();
        //    //    }
        //    //}
        //}

    }
}