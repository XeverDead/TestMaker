using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.TaskTypes
{
    class MultipleChoice: ITask
    {
        public string Question { get; protected set; }
        public List<string> Options { get; protected set; }
        public List<int> RightAnswersIndexes { get; protected set; }
        public int Time { get; protected set; }
        public bool IsTimeLimited { get; protected set; }
        public int Mark { get; protected set; }

        public MultipleChoice(string question, List<string> options, List<int> rightAnswersIndexes, int mark)
        {
            Question = question;

            if (options is null)
            {
                Options = new List<string>();
            }
            else
            {
                Options = new List<string>(options);
            }

            if (rightAnswersIndexes is null)
            {
                RightAnswersIndexes = new List<int>();
            }
            else
            {
                RightAnswersIndexes = new List<int>(rightAnswersIndexes);
            }

            Mark = mark;
            Time = 0;
            IsTimeLimited = false;
        }

        [JsonConstructor]
        public MultipleChoice(int time, string question, List<string> options, List<int> rightAnswersIndexes, int mark)
            : this(question, options, rightAnswersIndexes, mark)
        {
            if (time > 0)
            {
                Time = time;
                IsTimeLimited = true;
            }
        }
    }
}
