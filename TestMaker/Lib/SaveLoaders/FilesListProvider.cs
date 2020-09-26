using Lib.ResultTypes;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lib.SaveLoaders
{
    public class FilesListProvider
    {
        private readonly Dictionary<Type, string> extensions;

        public FilesListProvider()
        {
            extensions = new Dictionary<Type, string>
            {
                [typeof(Test)] = ".tmt",
                [typeof(TestResult)] = ".tmr"
            };
        }

        public List<string> GetFilesOfType(Type type)
        {
            if (extensions.ContainsKey(type))
            {
                if (!Directory.Exists("Tests"))
                {
                    Directory.CreateDirectory("Tests");
                }
                return GetFilesOfTypeFromDirectory(type, new DirectoryInfo("Tests"));
            }

            return null;
        }

        private List<string> GetFilesOfTypeFromDirectory(Type type, DirectoryInfo directory)
        {
            var pathsList = new List<string>();

            foreach (var file in directory.GetFiles())
            {
                if (file.Extension == extensions[type])
                {
                    pathsList.Add(file.FullName);
                }
            }

            foreach (var subDirectory in directory.GetDirectories())
            {
                if (subDirectory.Attributes != FileAttributes.Device)
                {
                    pathsList.AddRange(GetFilesOfTypeFromDirectory(type, subDirectory));
                }
            }

            return pathsList;
        }
    }
}
