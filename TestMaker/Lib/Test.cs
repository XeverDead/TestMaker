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
    }
}
