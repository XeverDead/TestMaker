using System.Collections.Generic;

namespace Lib
{
    public class Test
    {
        public string Name { get; set; }
        public int Time { get; set; }
        public bool IsTimeLimited => Time > 0;
        public List<Topic> Topics { get; set; }
        public string Password { get; set; }
        public bool HasPassword => !string.IsNullOrWhiteSpace(Password);

        public Test()
        {
            Name = string.Empty;
            Topics = new List<Topic>();
            Password = string.Empty;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
