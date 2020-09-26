using System.Collections.Generic;

namespace Lib.ResultTypes
{
    public class TestResult
    { 
        public Test Test { get; protected set; }
        public List<TaskResult> TaskResults { get; protected set; }
        public double Mark { get; protected set; }
        public string StudentName { get; set; }

        public TestResult(Test test, List<TaskResult> taskResults, double mark, string studentName)
        {
            Test = test;
            Mark = mark;
            StudentName = studentName;

            TaskResults = taskResults == null ? new List<TaskResult>() : new List<TaskResult>(taskResults);
        }
    }
}
