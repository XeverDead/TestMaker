namespace Lib
{
    public interface ISaveLoad
    {
        void Save(Test test);

        Test Load(string path);
    }
}
