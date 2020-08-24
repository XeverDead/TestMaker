using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.TaskTypes
{
    class MultipleChoice: ITask
    {
        public string Question { get; protected set; }
        public List<string> Options { get; protected set; }
        public List<string> RightAnswers { get; protected set; }
        public int Time { get; protected set; }
        public bool IsTimeLimited { get; protected set; }
        public int Mark { get; protected set; }

        public MultipleChoice(string question, List<string> options, List<string> rightAnswers, int mark)
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

            if (rightAnswers is null)
            {
                RightAnswers = new List<string>();
            }
            else
            {
                RightAnswers = new List<string>(rightAnswers);
            }

            Mark = mark;
            Time = 0;
            IsTimeLimited = false;
        }

        [JsonConstructor]
        public MultipleChoice(int time, string question, List<string> options, List<string> rightAnswers, int mark)
            : this(question, options, rightAnswers, mark)
        {
            if (time > 0)
            {
                Time = time;
                IsTimeLimited = true;
            }
        }
    }
}
