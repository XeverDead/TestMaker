namespace Lib.ResultTypes
{
    public class TaskResult
    {
        public Task Task { get; protected set; }
        public double Mark { get; protected set; }
        public dynamic Answer { get; protected set; }

        public TaskResult(Task task, dynamic answer)
        {
            Task = task;
            Answer = answer;
        }
    }
}
