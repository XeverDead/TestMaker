namespace Lib.SaveLoaders
{
    public interface IDataProvider<TData>
    {
        void Save(TData data);

        TData Load();
    }
}
