using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib
{
    public class Topic
    {
        public string Name { get; private set; }
        public List<Topic> SubTopics { get; private set; }
        public List<ITask> Tasks { get; private set; }
        public bool HasSubTopics { get; private set; }

        public Topic(string name, List<ITask> tasks)
        {
            Name = name;
            SubTopics = new List<Topic>();
            HasSubTopics = false;

            if (tasks == null)
            {
                Tasks = new List<ITask>();
            }
            else
            {
                Tasks = new List<ITask>(tasks);
            }
        }

        [JsonConstructor]
        public Topic(string name, List<ITask> tasks, List<Topic> subTopics)
            : this(name, tasks)
        {
            if (subTopics != null)
            {
                SubTopics = new List<Topic>(subTopics);
                HasSubTopics = true;
            }
        }
    }
}
