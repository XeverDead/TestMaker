using Lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    class DefaultRedactingCore
    {
        private Test test;

        public DefaultRedactingCore()
        {
            test = new Test();
        }

        public DefaultRedactingCore(IDataProvider<Test> testProvider)
        {
            test = testProvider.Load();
        }

        public Test GetTest()
        {
            return test;
        }
    }
}
