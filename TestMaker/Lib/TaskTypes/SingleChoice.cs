using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.TaskTypes
{
    public class SingleChoice : ITask
    {
        public string Question { get; protected set; }
        public List<string> Options { get; protected set; }
        public int RightAnswerIndex { get; protected set; }
        public int Time { get; protected set; }
        public bool IsTimeLimited { get; protected set; }
        public int Mark { get; protected set; }

        public SingleChoice(string question, List<string> options, int rightAnswerIndex, int mark)
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

            RightAnswerIndex = rightAnswerIndex;
            Mark = mark;
            Time = 0;
            IsTimeLimited = false;
        }

        [JsonConstructor]
        public SingleChoice(int time, string question, List<string> options, int rightAnswerIndex, int mark)
            : this(question, options, rightAnswerIndex, mark)
        {
            if (time > 0)
            {
                Time = time;
                IsTimeLimited = true;
            }
        }
    }
}
