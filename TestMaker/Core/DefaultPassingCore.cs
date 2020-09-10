using System;
using System.Collections.Generic;
using Lib;

namespace Core
{
    public class DefaultPassingCore
    {
        public Test CurrentTest { get; protected set; }
        public List<Topic> AllTopics { get; protected set; }
        public Topic CurrentTopic { get; protected set; }
        public Task CurrentTask { get; protected set; }
        public ISaveLoad SaveLoad { get; protected set; }

        protected int currentTopicIndex;
        protected int currentTaskIndex;

        public DefaultPassingCore(string pathToTest, ISaveLoad saveLoad)
        {
            SaveLoad = saveLoad;
            CurrentTest = SaveLoad.Load(pathToTest);

            AllTopics = new List<Topic>();
            GetAllTopics();

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
    }
}
