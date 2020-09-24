using Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core
{
    public class DefaultRedactingCore
    {
        private Test test;
        private IDataProvider<Test> testProvider;

        private bool isNewTest;

        public DefaultRedactingCore(IDataProvider<Test> testProvider, bool isNewTest)
        {
            this.testProvider = testProvider;
            this.isNewTest = isNewTest;
        }

        public Test GetTest(out bool wasTestLoaded)
        {
            wasTestLoaded = true;

            if (isNewTest)
            {
                test = new Test();
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

            return test;
        }

        public void SaveTest(Test test)
        {
            testProvider.Save(test);
        }

        public List<Type> GetAllTaskTypes()
        {
            var types = Assembly.LoadFrom("Lib.dll").GetTypes().ToList();

            var taskTypes = new List<Type>();
            foreach (var type in types)
            {
                if (type.Namespace == "Lib.TaskTypes" && !type.IsAbstract)
                {
                    taskTypes.Add(type);
                }
            }

            return taskTypes;
        }
    }
}
