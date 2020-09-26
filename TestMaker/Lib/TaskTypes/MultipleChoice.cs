using System.Collections.Generic;

namespace Lib.TaskTypes
{
    public class MultipleChoice: Task
    {
        public List<string> Options { get; set; }
        public List<int> RightAnswersIndexes { get; set; }

        public MultipleChoice()
        {
            Options = new List<string>();
            RightAnswersIndexes = new List<int>();
        }

        public override double CountMark(dynamic answerIndexes)
        {
            if (answerIndexes == null)
            {
                return 0.0;
            }

            var mark = 0.0;

            var additionForRightAnswer = Mark / RightAnswersIndexes.Count;
            var subtractionForWrongAnswer = Mark / (Options.Count - RightAnswersIndexes.Count);

            foreach (var rightIndex in RightAnswersIndexes)
            {
                if (answerIndexes.Contains(rightIndex))
                {
                    mark += additionForRightAnswer;
                }
                else
                {
                    mark -= subtractionForWrongAnswer;
                }
            }

            if (mark < 0)
            {
                mark = 0;
            }

            return mark;
        }
    }
}
