using System.Collections.Generic;

namespace Lib.ResultTypes
{
    public class TestResult
    {
        public Test Test { get; protected set; }
        public List<TaskResult<Task>> TaskResults { get; protected set; }
        public double Mark { get; protected set; }

        public TestResult(Test test, List<TaskResult<Task>> taskResults, double mark)
        {
            Test = test;
            Mark = mark;

            if (taskResults == null)
            {
                TaskResults = new List<TaskResult<Task>>();
            }
            else
            {
                TaskResults = new List<TaskResult<Task>>(taskResults);
            }
        }
    }
}
