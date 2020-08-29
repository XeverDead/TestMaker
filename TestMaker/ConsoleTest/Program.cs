using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib;
using Lib.TaskTypes;
using UI;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var question1 = "lol";
            var options1 = new List<string>
            {
                "lol",
                "ne lol"
            };
            var index1 = 0;
            var task1 = new SingleChoice(question1, options1, index1, index1);

            var question2 = "kek";
            var options2 = new List<string>
            {
                "ne kek",
                "kek"
            };
            var index2 = 1;
            var task2 = new SingleChoice(question2, options2, index2, index2);

            var tasks = new List<ITask>()
            {
                task1,
                task2
            };

            var subTopic1 = new Topic("lol", tasks);
            var subTopicList = new List<Topic>()
            {
                subTopic1
            };

            var subTopic2 = new Topic("kek", tasks);

            var topic1 = new Topic("prelol", null, subTopicList);

            subTopicList = new List<Topic>()
            {
                subTopic2,
                subTopic1
            };
            var topic2 = new Topic("prekek", null, subTopicList);

            var topicList = new List<Topic>()
            {
                topic2,
                topic1
            };

            var test = new Test("Test", topicList);

            SaveLoad.Save(test);

            test = SaveLoad.Load("Test");

            Console.WriteLine(test.Topics[0].Name);
        }
    }
}
