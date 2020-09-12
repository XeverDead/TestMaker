using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib
{
    public class Test
    {
        public string Name { get; set; }
        public int Time { get; set; }
        public bool IsTimeLimited { get => Time > 0; }
        public List<Topic> Topics { get; set; }

        //public Test(string name, List<Topic> topics)
        //{
        //    Name = name;

        //    if (topics is null)
        //    {
        //        Topics = new List<Topic>();
        //    }
        //    else
        //    {
        //        Topics = new List<Topic>(topics);
        //    }

        //    Time = 0;
        //    IsTimeLimited = false;
        //}

        //[JsonConstructor]
        //public Test(int time, string name, List<Topic> topics)
        //    : this(name, topics)
        //{
        //    if (time > 0)
        //    {
        //        Time = time;
        //        IsTimeLimited = true;
        //    }
        //}
    }
}
