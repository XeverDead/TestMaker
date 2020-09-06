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

        private int currentTopicIndex;
        private int currentTaskIndex;

        public DefaultPassingCore(string pathToTest)
        {
            var loader = new SaveLoad();
            CurrentTest = loader.Load(pathToTest);

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

        public bool SetNewCurrentTask()
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
                        CurrentTask = AllTopics[topicIndex].Tasks[0];

                        currentTaskIndex = 0;
                        currentTopicIndex = topicIndex;

                        hasNextTask = true;
                        break;
                    }
                }
            }

            return hasNextTask;
        }
    }
}
