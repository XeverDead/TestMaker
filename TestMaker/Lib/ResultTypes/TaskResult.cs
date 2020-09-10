namespace Lib.ResultTypes
{
    public abstract class TaskResult<TTask> where TTask: Task
    {
        public TTask Task { get; protected set; }
        public double Mark { get; protected set; }
        public dynamic AnswerIndex { get; protected set; }

        public TaskResult(TTask task, dynamic answerIndex, double mark)
        {
            Task = task;
            Mark = mark;
            AnswerIndex = answerIndex;
        }
    }
}
