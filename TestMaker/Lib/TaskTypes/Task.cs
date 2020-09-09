using System.Collections.Generic;

namespace Lib
{
    public abstract class Task
    {
        public string Question { get; protected set; }
        public int Time { get; protected set; }
        public bool IsTimeLimited { get; protected set; }
        public double Mark { get; protected set; }

        public Task(string question, List<string> options, double mark)
        {
            Question = question;
            Mark = mark;
            Time = 0;
            IsTimeLimited = false;
        }

        public Task(int time, string question, List<string> options, double mark)
            : this(question, options, mark)
        {
            if (time > 0)
            {
                Time = time;
                IsTimeLimited = true;
            }
        }
    }
}
