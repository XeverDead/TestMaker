using Lib.TaskTypes;
using System.Collections.Generic;

namespace Lib
{
    public class TaskTopicTestView
    {
        public Dictionary<Task, Topic> TasksAndTopics { get; set; }
        public Test Test { get; set; }
    }
}
