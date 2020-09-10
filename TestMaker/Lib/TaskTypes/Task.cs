using System.Collections.Generic;

namespace Lib
{
    public abstract class Task
    {
        public string Question { get; set; }
        public int Time { get; set; }
        public bool IsTimeLimited { get; set; }
        public double Mark { get; set; }

        //public Task(string question, List<string> options, double mark)
        //{
        //    Question = question;
        //    Mark = mark;
        //    Time = 0;
        //    IsTimeLimited = false;
        //}

        //public Task(int time, string question, List<string> options, double mark)
        //    : this(question, options, mark)
        //{
        //    if (time > 0)
        //    {
        //        Time = time;
        //        IsTimeLimited = true;
        //    }
        //}
    }
}
