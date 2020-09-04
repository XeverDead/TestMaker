using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.TaskTypes
{
    public class SingleChoice : Task
    {
        public List<string> Options { get; protected set; }
        public int RightAnswerIndex { get; protected set; }

        public SingleChoice(string question, List<string> options, int rightAnswerIndex, int mark)
            : base(question, options, mark)
        {
            if (options is null)
            {
                Options = new List<string>();
            }
            else
            {
                Options = new List<string>(options);
            }

            RightAnswerIndex = rightAnswerIndex;
        }

        [JsonConstructor]
        public SingleChoice(int time, string question, List<string> options, int rightAnswerIndex, int mark)
            : base(time, question, options, mark) 
        {
            if (options is null)
            {
                Options = new List<string>();
            }
            else
            {
                Options = new List<string>(options);
            }

            RightAnswerIndex = rightAnswerIndex;
        }
    }
}
