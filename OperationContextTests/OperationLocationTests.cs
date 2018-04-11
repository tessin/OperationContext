using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tessin.Diagnostics.Internal;

namespace Tessin.Diagnostics
{
    [TestClass]
    public class OperationLocationTests
    {
        public enum X
        {
            None
        }

        [TestMethod]
        public void OperationLocation_ToString_Test1()
        {
            var context = new OperationContext(state: OperationValueDictionary.Empty.SetItem(OperationValueKey.Create(X.None), 1));
            var stack = OperationContext.Backtrace(context);
            Trace.WriteLine(string.Join("", stack));
        }
    }
}
