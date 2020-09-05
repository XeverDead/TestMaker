using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib
{
    public class Topic
    {
        public string Name { get; private set; }
        public List<Topic> SubTopics { get; private set; }
        public List<Task> Tasks { get; private set; }
        public bool HasSubTopics { get; private set; }
        public bool HasTasks { get; private set; }

        public Topic(string name, List<Task> tasks)
        {
            Name = name;
            SubTopics = new List<Topic>();
            HasSubTopics = false;

            if (tasks == null || tasks.Count == 0)
            {
                Tasks = new List<Task>();
                HasTasks = false;
            }
            else
            {
                Tasks = new List<Task>(tasks);
                HasTasks = true;
            }
        }

        [JsonConstructor]
        public Topic(string name, List<Task> tasks, List<Topic> subTopics)
            : this(name, tasks)
        {
            if (subTopics != null && subTopics.Count > 0)
            {
                SubTopics = new List<Topic>(subTopics);
                HasSubTopics = true;
            }
        }
    }
}
