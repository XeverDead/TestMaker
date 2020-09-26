using Lib.SaveLoaders;
using System.Collections.Generic;

namespace Core
{
    public class HubCore
    {
        private readonly FilesListProvider filesProvider;

        public HubCore()
        {
            filesProvider = new FilesListProvider();
        }

        public List<string> GetFiles<TFileType>()
        {
            return filesProvider.GetFilesOfType(typeof(TFileType));
        }
    }
}
