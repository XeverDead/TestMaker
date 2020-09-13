using Lib;
using Lib.ResultTypes;
using Lib.TaskTypes;
using System;
using System.Collections.Generic;

namespace Core
{
    public class DefaultPassingCore
    {
        public Test CurrentTest { get; protected set; }
        public List<Topic> AllTopics { get; protected set; }
        public Topic CurrentTopic { get; protected set; }
        public Task CurrentTask { get; protected set; }

        public string StudentName { get; set; }

        protected IDataProvider<Test> testProvider;
        protected Dictionary<Task, TaskResult> taskResults;
        protected TestResult testResult;

        protected int currentTopicIndex;
        protected int currentTaskIndex;

        public DefaultPassingCore(IDataProvider<Test> testProvider)
        {
            this.testProvider = testProvider;
            CurrentTest = testProvider.Load();

            AllTopics = new List<Topic>();
            GetAllTopics();

            taskResults = new Dictionary<Task, TaskResult>();
            AddTasksToResults();

            for (var topicIndex = 0; topicIndex < AllTopics.Count; topicIndex++) 
            {
                if (AllTopics[topicIndex].HasTasks)
                {
                    CurrentTopic = AllTopics[topicIndex];
                    CurrentTask = AllTopics[topicIndex].Tasks[0];

                    currentTaskIndex = 0;
                    currentTopicIndex = topicIndex;

                    break;
                }
            }
        }

        private void GetAllTopics()
        {
            foreach (var topic in CurrentTest.Topics)
            {
                AllTopics.Add(topic);

                if (topic.HasSubTopics)
                {
                    GetAllSubTopics(topic);
                }
            }
        }

        private void AddTasksToResults()
        {
            foreach (var topic in AllTopics)
            {
                if (topic.HasTasks)
                {
                    foreach (var task in topic.Tasks)
                    {
                        taskResults[task] = null;
                    }
                }
            }
        }

        private void GetAllSubTopics(Topic topic)
        {
            foreach (var subTopic in topic.SubTopics)
            {
                AllTopics.Add(subTopic);

                if (subTopic.HasSubTopics)
                {
                    GetAllSubTopics(subTopic);
                }
            }
        }

        public bool SetNextTaskToCurrent()
        {
            var hasNextTask = false;

            if (CurrentTopic.Tasks.Count > currentTaskIndex + 1)
            {
                CurrentTask = CurrentTopic.Tasks[++currentTaskIndex];
                hasNextTask = true;
            }
            else
            {
                for (var topicIndex = currentTopicIndex + 1; topicIndex < AllTopics.Count; topicIndex++)
                {
                    if (AllTopics[topicIndex].HasTasks)
                    {
                        CurrentTopic = AllTopics[topicIndex];
                        CurrentTask = CurrentTopic.Tasks[0];

                        currentTaskIndex = 0;
                        currentTopicIndex = topicIndex;    

                        hasNextTask = true;
                        break;
                    }
                }
            }

            return hasNextTask;
        }

        public bool SetPrevTaskToCurrent()
        {
            var hasPrevTask = false;

            if (currentTaskIndex > 0)
            {
                CurrentTask = CurrentTopic.Tasks[--currentTaskIndex];
                hasPrevTask = true;
            }
            else
            {
                for (var topicIndex = currentTopicIndex - 1; topicIndex >= 0; topicIndex--)
                {
                    if (AllTopics[topicIndex].HasTasks)
                    {
                        CurrentTopic = AllTopics[topicIndex];
                        CurrentTask = CurrentTopic.Tasks[^1];

                        currentTopicIndex = topicIndex;
                        currentTaskIndex = CurrentTopic.Tasks.Count - 1;

                        hasPrevTask = true;
                        break;
                    }
                }
            }

            return hasPrevTask;
        }

        public void SetResult(Task task, dynamic answer)
        {
            taskResults[task] = new TaskResult(CurrentTask, answer);
        }

        public double GetTestMark(out double maxMark)
        {
            var testMark = 0.0;
            maxMark = 0.0;

            foreach (var task in taskResults.Keys)
            {
                maxMark += task.Mark;

                if (taskResults[task] is null)
                {
                    continue;
                }

                if (task is SingleChoice scTask)
                {
                    testMark += GetSingleChoiceMark(scTask, taskResults[task]);
                }
                else if (task is MultipleChoice mcTask)
                {
                    testMark += GetMultipleChoiceMark(mcTask, taskResults[task]);
                }
            }

            testResult = new TestResult(CurrentTest, taskResults, testMark);
            if (StudentName == null)
            {
                StudentName = "Unknown";
            }
            testResult.StudentName = StudentName;

            return testMark;
        }

        private double GetSingleChoiceMark(SingleChoice task, TaskResult taskResult)
        {
            var answerIndex = (int)taskResult.Answer;
            var mark = 0.0;

            if (answerIndex == task.RightAnswerIndex)
            {
                mark = task.Mark;
            }

            return mark;
        }

        private double GetMultipleChoiceMark(MultipleChoice task, TaskResult taskResult)
        {
            var answerIndexes = (List<int>)taskResult.Answer;
            var mark = 0.0;

            var additionForRightAnswer = task.Mark / task.RightAnswersIndexes.Count;
            var subtractionForWrongAnswer = task.Mark / (task.Options.Count - task.RightAnswersIndexes.Count);

            foreach (var rightIndex in task.RightAnswersIndexes)
            {
                if (answerIndexes.Contains(rightIndex))
                {
                    mark += additionForRightAnswer;
                }
                else
                {
                    mark -= subtractionForWrongAnswer;
                }
            }

            if (mark < 0)
            {
                mark = 0;
            }

            return mark;
        }

        public void SaveResult(IDataProvider<TestResult> resultProvider)
        {
            if (testResult != null)
            {
                resultProvider.Save(testResult);
            }
        }

        public bool WasAnswerGiven(out dynamic answer)
        {
            answer = null;

            if (taskResults[CurrentTask] != null) 
            {
                answer = taskResults[CurrentTask].Answer;
                return true;
            }

            return false;
        }
    }
}
