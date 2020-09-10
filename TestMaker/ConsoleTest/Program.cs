using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Lib;
using Lib.TaskTypes;
using UI;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var question1 = "first";
            var options1 = new List<string>
            {
                "lol",
                "ne lol"
            };
            var index1 = 0;
            var task1 = new SingleChoice
            {
                Question = question1,
                Options = options1,
                RightAnswerIndex = index1,
                Mark = index1,
            };

            var question2 = "second";
            var options2 = new List<string>
            {
                "ne kek",
                "kek"
            };
            var index2 = 1;
            var task2 = new SingleChoice
            {
                Question = question2,
                Options = options2,
                RightAnswerIndex = index2,
                Mark = index2,
            };

            var tasks = new List<Lib.Task>()
            {
                task1,
                task2
            };

            var subTopic1 = new Topic("first sub", tasks);
            var subTopicList = new List<Topic>()
            {
                subTopic1
            };

            var subTopic2 = new Topic("second sub", tasks);

            var topic1 = new Topic("has 1", null, subTopicList);

            subTopicList = new List<Topic>()
            {
                subTopic2,
                subTopic1
            };
            var topic2 = new Topic("has 2", null, subTopicList);

            var topicList = new List<Topic>()
            {
                topic2,
                topic1
            };

            var test = new Test("Test", topicList);

            new SaveLoad().Save(test);

            var core = new DefaultPassingCore("Test.tmt", new SaveLoad());

            Console.WriteLine(core.CurrentTopic.Name + " -> " + core.CurrentTask.Question);

            while (core.SetNextTaskToCurrent())
            {
                Console.WriteLine(core.CurrentTopic.Name + " -> " + core.CurrentTask.Question);
            }
        }
    }
}
