﻿using Newtonsoft.Json;
using System.IO;

namespace Lib
{
    public class SaveLoad: ISaveLoad
    {
        private JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        public void Save(Test test)
        {
            using (var writer = new StreamWriter($"D:\\{test.Name}.tmt"))
            {
                writer.WriteLine(JsonConvert.SerializeObject(test, serializerSettings));
            }
        }

        public Test Load(string testName)
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
