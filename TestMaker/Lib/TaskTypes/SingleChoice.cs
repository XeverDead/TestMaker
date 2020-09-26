using System.Collections.Generic;

namespace Lib.TaskTypes
{
    public class SingleChoice : Task
    {
        public List<string> Options { get; set; }
        public int RightAnswerIndex { get; set; }

        public SingleChoice()
        {
            Options = new List<string>();
        }

        public override double CountMark(dynamic answerIndex)
        {
            if (answerIndex.GetType() == RightAnswerIndex.GetType() && answerIndex == RightAnswerIndex) 
            {
                return Mark;
            }

            return 0.0;
        }
    }
}
