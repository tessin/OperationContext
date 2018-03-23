using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tessin.Diagnostics
{
    [TestClass]
    public class OperationLocationTests
    {
        [TestMethod]
        public void OperationLocation_ToString_Test1()
        {
            var context = new OperationContext(state: OperationValueDictionary.Empty.SetItem("test", 1));
            var stack = OperationContext.Backtrace(context);
            Trace.WriteLine(string.Join("", stack));
        }
    }
}
