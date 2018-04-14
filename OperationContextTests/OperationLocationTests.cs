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
            var context = new OperationContext();
            context = context.WithValue(X.None, 1);
            var stack = OperationContext.Backtrace(context);
            Trace.WriteLine(string.Join("", stack));
        }
    }
}
