using System.Collections.Generic;
using Lib.TaskTypes;
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

        public Topic()
        {
            SubTopics = new List<Topic>();
            Tasks = new List<Task>();
            Name = string.Empty;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
