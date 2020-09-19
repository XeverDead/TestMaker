using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib
{
    public class Topic
    {
        public string Name { get; set; }
        public List<Topic> SubTopics { get; set; }
        public List<Task> Tasks { get; set; }
        public bool HasSubTopics
        {
            get
            {
                return SubTopics != null && SubTopics.Count != 0;
            }
        }
        public bool HasTasks
        {
            get
            {
                return Tasks != null && Tasks.Count != 0;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
