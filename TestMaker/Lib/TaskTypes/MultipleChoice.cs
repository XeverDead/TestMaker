﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.TaskTypes
{
    public class MultipleChoice: Task
    {
        public List<string> Options { get; set; }
        public List<int> RightAnswersIndexes { get; set; }
    }
}
