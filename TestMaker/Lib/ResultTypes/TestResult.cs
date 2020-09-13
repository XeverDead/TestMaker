using System.Collections.Generic;

namespace Lib.ResultTypes
{
    public class TestResult
    {
        public Test Test { get; protected set; }
        public Dictionary<Task, TaskResult> TaskResults { get; protected set; }
        public double Mark { get; protected set; }
        public string StudentName { get; set; }

        public TestResult(Test test, Dictionary<Task, TaskResult> taskResults, double mark)
        {
            Test = test;
            Mark = mark;

            if (taskResults == null)
            {
                TaskResults = new Dictionary<Task, TaskResult>();
            }
            else
            {
                TaskResults = new Dictionary<Task, TaskResult>(taskResults);
            }
        }
    }
}
