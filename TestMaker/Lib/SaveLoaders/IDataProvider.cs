using Lib.ResultTypes;

namespace Lib
{
    public interface IDataProvider<TData>
    {
        void Save(TData data);

        TData Load();
    }
}
