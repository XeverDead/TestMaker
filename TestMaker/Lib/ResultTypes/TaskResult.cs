using Lib.TaskTypes;

namespace Lib.ResultTypes
{
    public class TaskResult
    {
        public Task Task { get; protected set; }
        public double Mark { get; set; }
        public dynamic Answer { get; set; }

        public TaskResult(Task task, dynamic answer)
        {
            Task = task;
            Answer = answer;
        }
    }
}
