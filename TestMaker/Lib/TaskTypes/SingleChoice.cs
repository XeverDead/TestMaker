using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.TaskTypes
{
    public class SingleChoice : ITask
    {
        public string Question { get; protected set; }
        public List<string> Options { get; protected set; }
        public string RightAnswer { get; protected set; }
        public int Time { get; protected set; }
        public bool IsTimeLimited { get; protected set; }
        public int Mark { get; protected set; }

        public SingleChoice(string question, List<string> options, string rightAnswer, int mark)
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

            RightAnswer = rightAnswer;
            Mark = mark;
            Time = 0;
            IsTimeLimited = false;
        }

        [JsonConstructor]
        public SingleChoice(int time, string question, List<string> options, string rightAnswer, int mark)
            : this(question, options, rightAnswer, mark)
        {
            if (time > 0)
            {
                Time = time;
                IsTimeLimited = true;
            }
        }
    }
}
