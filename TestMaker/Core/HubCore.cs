using Lib.SaveLoaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class HubCore
    {
        private FilesListProvider filesProvider;

        public HubCore()
        {
            filesProvider = new FilesListProvider();
        }

        public List<string> GetFiles<FileType>()
        {
            return filesProvider.GetFilesOfType(typeof(FileType));
        }
    }
}
