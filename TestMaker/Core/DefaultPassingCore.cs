using Lib;
using Lib.ResultTypes;
using Lib.TaskTypes;
using System;
using System.Collections.Generic;

namespace Core
{
    public class DefaultPassingCore
    {
        private Test test;

        public DefaultPassingCore(IDataProvider<Test> taskProvider)
        {
            test = taskProvider.Load();
        }

        public Dictionary<Task, Topic> GetTest()
        {
            var tasksAndTopics = new Dictionary<Task, Topic>();

            foreach (var topic in test.Topics)
            {
                foreach (var pair in GetTasksFromTopic(topic))
                {
                    tasksAndTopics.Add(pair.Key, pair.Value);
                }
            }

            return tasksAndTopics;
        }

        private Dictionary<Task, Topic> GetTasksFromTopic(Topic topic)
        {
            var tasksAndTopics = new Dictionary<Task, Topic>();

            if (topic.HasSubTopics)
            {
                foreach (var subTopic in topic.SubTopics)
                {
                    if (subTopic.HasTasks)
                    {
                        foreach (var task in subTopic.Tasks)
                        {
                            tasksAndTopics.Add(task, subTopic);
                        }
                    }

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

        public double CountTestMark(ref List<TaskResult> results, out double maxMark)
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

                if (result.Task is SingleChoice scTask)
                {
                    var taskMark = CountSingleChoiceMark(scTask, result.Answer);

                    mark += taskMark;
                    result.Mark = taskMark;
                }
                else if (result.Task is MultipleChoice mcTask)
                {
                    var taskMark = CountMultipleChoiceMark(mcTask, result.Answer);

                    mark += taskMark;
                    result.Mark = taskMark;
                }
            }

            return mark;
        }

        private double CountSingleChoiceMark(SingleChoice task, int answerIndex)
        {
            if (task.RightAnswerIndex == answerIndex)
            {
                return task.Mark;
            }

            return 0;
        }

        private double CountMultipleChoiceMark(MultipleChoice task, List<int> answerIndexes)
        {
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
    }
}
