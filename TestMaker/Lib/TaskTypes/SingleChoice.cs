using System.Collections.Generic;
using Newtonsoft.Json;

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

        public override double CountMark(dynamic answerIndex) => RightAnswerIndex == answerIndex ? Mark : 0.0;
    }
}
