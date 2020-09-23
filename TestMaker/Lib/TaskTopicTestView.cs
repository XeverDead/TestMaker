using Lib.TaskTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib
{
    public class TaskTopicTestView
    {
        public Dictionary<Task, Topic> TasksAndTopics { get; set; }
        public Test Test { get; set; }
    }
}
