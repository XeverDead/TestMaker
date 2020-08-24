using System.Collections.Generic;

namespace Lib
{
    public interface ITask
    {
        string Question { get; }
        List<string> Options { get; }
        int Time { get; }
        bool IsTimeLimited { get; }
        int Mark { get; }
    }
}
