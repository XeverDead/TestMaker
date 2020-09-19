using System.Collections.Generic;

namespace Lib.ResultTypes
{
    public class TestResult
    {
        public Test Test { get; protected set; }
        public List<TaskResult> TaskResults { get; protected set; }
        public double Mark { get; protected set; }
        public string StudentName { get; set; }

        public TestResult(Test test, List<TaskResult> taskResults, double mark)
        {
            Test = test;
            Mark = mark;

            if (taskResults == null)
            {
                TaskResults = new List<TaskResult>();
            }
            else
            {
                TaskResults = new List<TaskResult>(taskResults);
            }
        }
    }
}
