using Lib;
using Lib.ResultTypes;
using Lib.TaskTypes;
using System.Collections.Generic;

namespace Core
{
    public class DefaultPassingCore
    {
        private Test test;
        private TestResult testResult;

        private IDataProvider<Test> testProvider;
        private IDataProvider<TestResult> resultProvider;

        private bool isForShowingResults;

        public DefaultPassingCore(IDataProvider<Test> testProvider, IDataProvider<TestResult> resultProvider, bool isForShowingResults)
        {
            this.testProvider = testProvider;
            this.resultProvider = resultProvider;

            this.isForShowingResults = isForShowingResults;
        }

        public TaskTopicTestView GetTest(out bool wasTestLoaded)
        {
            wasTestLoaded = true;

            if (isForShowingResults)
            {
                if (testResult == null)
                {
                    GetResults(out bool wereResultsLoaded);

                    wasTestLoaded = wereResultsLoaded;
                }              

                test = testResult.Test;
            }
            else
            {
                try
                {
                    test = testProvider.Load();
                }
                catch
                {
                    wasTestLoaded = false;
                }
            }

            if (!wasTestLoaded)
            {
                return null;
            }

            var tasksAndTopics = new Dictionary<Task, Topic>();

            foreach (var topic in test.Topics)
            {
                foreach (var pair in GetTasksFromTopic(topic))
                {
                    tasksAndTopics.Add(pair.Key, pair.Value);
                }
            }

            var testView = new TaskTopicTestView()
            {
                Test = test,
                TasksAndTopics = tasksAndTopics
            };

            return testView;
        }

        private Dictionary<Task, Topic> GetTasksFromTopic(Topic topic)
        {
            var tasksAndTopics = new Dictionary<Task, Topic>();

            if (topic.HasTasks)
            {
                foreach (var task in topic.Tasks)
                {
                    tasksAndTopics.Add(task, topic);
                }
            }

            if (topic.HasSubTopics)
            {
                foreach (var subTopic in topic.SubTopics)
                {
                    if (subTopic.HasSubTopics)
                    {
                        foreach (var pair in GetTasksFromTopic(subTopic))
                        {
                            tasksAndTopics.Add(pair.Key, pair.Value);
                        }
                    }
                }
            }

            return tasksAndTopics;
        }

        public void SetMarksToResults(ref List<TaskResult> results, out double maxMark)
        {
            var mark = 0.0;
            maxMark = 0.0;

            foreach (var result in results)
            {
                maxMark += result.Task.Mark;

                if (result.Answer == null) 
                {
                    result.Mark = 0;
                    continue;
                }

                mark += result.Task.CountMark(result.Answer);
            }
        }

        public void SaveResult(List<TaskResult> results, string studentName)
        {
            var mark = 0.0;

            foreach (var result in results)
            {
                if (result.Answer == null)
                {
                    result.Mark = 0;
                    continue;
                }

                mark += result.Task.CountMark(result.Answer);
            }

            testResult = new TestResult(test, results, mark, studentName);

            resultProvider.Save(testResult);
        }

        public List<TaskResult> GetResults(out bool wereResultsLoaded)
        {
            wereResultsLoaded = true;

            try
            {
                testResult = resultProvider.Load();
            }
            catch
            {
                wereResultsLoaded = false;

                return null;
            }

            return testResult.TaskResults;
        }
    }
}
