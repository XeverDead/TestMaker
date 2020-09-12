namespace Lib
{
    public interface ITestProvider
    {
        void Save(Test test);

        Test Load();
    }
}
