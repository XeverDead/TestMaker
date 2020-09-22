using System.Collections.Generic;

namespace Lib
{
    public abstract class Task
    {
        public string Question { get; set; }
        public double Mark { get; set; }

        public abstract double CountMark(dynamic answer);

        public override string ToString()
        {
            return Question;
        }
    }
}
