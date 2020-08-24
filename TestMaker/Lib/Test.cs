using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib
{
    public class Test
    {
        public string Name { get; protected set; }
        public int Time { get; protected set; }
        public bool IsTimeLimited { get; protected set; }
        public List<ITask> Tasks { get; protected set; }

        public Test(string name, List<ITask> tasks)
        {
            Name = name;

            if (tasks is null)
            {
                Tasks = new List<ITask>();
            }
            else
            {
                Tasks = new List<ITask>(tasks);
            }

            Time = 0;
            IsTimeLimited = false;
        }

        [JsonConstructor]
        public Test(int time, string name, List<ITask> tasks)
            : this(name, tasks)
        {
            if (time > 0)
            {
                Time = time;
                IsTimeLimited = true;
            }
        }
    }
}
