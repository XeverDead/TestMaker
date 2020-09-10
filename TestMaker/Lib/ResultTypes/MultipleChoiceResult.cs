using Lib.TaskTypes;
using System.Collections.Generic;

namespace Lib.ResultTypes
{
    class MultipleChoiceResult : TaskResult<MultipleChoice>
    {
        public MultipleChoiceResult(MultipleChoice task, List<int> answerIndexes, double mark)
            : base(task, answerIndexes, mark) { }
    }
}
