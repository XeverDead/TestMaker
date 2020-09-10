using Lib.TaskTypes;

namespace Lib.ResultTypes
{
    public class SingleChoiceResult : TaskResult<SingleChoice>
    {
        public SingleChoiceResult(SingleChoice task, int answerIndex, double mark)
            : base(task, answerIndex, mark) { }
    }
}
