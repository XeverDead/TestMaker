using Lib.ResultTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lib
{
    public class JsonDataProvider<TData>: IDataProvider<TData>
    {
        private readonly string path;

        public JsonDataProvider(string path)
        {
            this.path = path;
        }

        private JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        public void Save(TData data)
        {
            using (var writer = new StreamWriter($"{path}"))
            {
                writer.WriteLine(JsonConvert.SerializeObject(data, serializerSettings));
            }
        }

        public TData Load()
        {
            TData data;

            using (var reader = new StreamReader($"{path}"))
            {
                var text = reader.ReadToEnd();
                data = JsonConvert.DeserializeObject<TData>(text, serializerSettings);
            }

            return data;
        }
    }
}
