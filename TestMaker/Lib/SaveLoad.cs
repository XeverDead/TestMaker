using Newtonsoft.Json;
using System.IO;
using Lib;

namespace Lib
{
    public static class SaveLoad
    {
        private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static void Save(Test test)
        {
            using (var writer = new StreamWriter($"D:\\{test.Name}.tmt"))
            {
                writer.WriteLine(JsonConvert.SerializeObject(test, serializerSettings));
            }
        }

        public static Test Load(string testName)
        {
            Test test;

            using (var reader = new StreamReader($"D:\\{testName}.tmt"))
            {
                var text = reader.ReadToEnd();
                test = JsonConvert.DeserializeObject<Test>(text, serializerSettings);
            }

            return test;
        }
    }
}
