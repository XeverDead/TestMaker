using System.IO;
using Newtonsoft.Json;

namespace Lib.SaveLoaders
{
    public class JsonDataProvider<TData>: IDataProvider<TData>
    {
        private readonly string path;

        public JsonDataProvider(string path)
        {
            this.path = path;
        }

        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        public void Save(TData data)
        {
            if (!Directory.Exists(path.Substring(0, path.LastIndexOf('\\'))))
            {
                Directory.CreateDirectory(path.Substring(0, path.LastIndexOf('\\')));
            }

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

            if (data == null)
            {
                throw new FileLoadException("File corrupted");
            }

            return data;
        }
    }
}
