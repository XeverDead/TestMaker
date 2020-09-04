﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.TaskTypes
{
    class MultipleChoice: Task
    {
        public List<string> Options { get; protected set; }
        public List<int> RightAnswersIndexes { get; protected set; }

        public MultipleChoice(string question, List<string> options, List<int> rightAnswersIndexes, int mark)
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

            if (rightAnswersIndexes is null)
            {
                RightAnswersIndexes = new List<int>();
            }
            else
            {
                RightAnswersIndexes = new List<int>(rightAnswersIndexes);
            }
        }

        [JsonConstructor]
        public MultipleChoice(int time, string question, List<string> options, List<int> rightAnswersIndexes, int mark)
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

            if (rightAnswersIndexes is null)
            {
                RightAnswersIndexes = new List<int>();
            }
            else
            {
                RightAnswersIndexes = new List<int>(rightAnswersIndexes);
            }
        }
    }
}
