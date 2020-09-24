using Lib.ResultTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lib.SaveLoaders
{
    public class FilesListProvider
    {
        private Dictionary<Type, string> extensions;

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
                var pathsList = new List<string>();

                var directory = new DirectoryInfo("D:\\Tests");

                foreach (var subDirectory in directory.GetDirectories())
                {
                    if (!(subDirectory.Attributes == FileAttributes.Device))
                    {
                        foreach (var file in subDirectory.GetFiles())
                        {
                            if (file.Extension == extensions[type])
                            {
                                pathsList.Add(file.FullName);
                            }
                        }
                    }
                }

                return pathsList;
            }

            return null;
        }
    }
}
