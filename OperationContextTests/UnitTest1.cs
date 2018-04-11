using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace Tessin.Diagnostics
{
    [TestClass]
    public class UnitTest1
    {
        enum State
        {
            X,
            Y,
            [EnumMember(Value = "i")]
            I
        }

        [TestMethod]
        public void TestMethod1()
        {
            var context = new OperationContext();
            Assert.AreEqual(nameof(TestMethod1), OperationContext.Backtrace(context)[0].Location.MemberName);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var context = new OperationContext();
            context = context.WithValue(State.X, 3);
            Assert.AreEqual(3, context.GetValue(State.X));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var context = new OperationContext();
            context = context.WithValue(State.X, 3).WithValue(State.Y, 5);
            Assert.AreEqual(5, context.GetValue(State.Y));
        }

        [TestMethod]
        public void TestMethod4()
        {
            TestMethod5(new OperationContext());
        }

        public void TestMethod5(OperationContext context)
        {
            Assert.AreEqual(nameof(TestMethod5), OperationContext.Backtrace(context.CreateScope())[0].Location.MemberName);
        }

        [TestMethod]
        public void TestMethod6()
        {
            var context = new OperationContext();

            for (int i = 0; i < 10; i++)
            {
                TestMethod7(i, context.CreateScope());
            }
        }

        private void TestMethod7(int i, OperationContext context)
        {
            context = context.WithValue(State.I, i);

            var json = JsonConvert.SerializeObject(context);

            var arr = JArray.Parse(json);

            Assert.AreEqual(i, (int)arr[0]["s"]["i"]);

            var id = BitConverter.ToString(OperationContext.Id(OperationContext.Backtrace(context)));

            Assert.AreEqual("F7-CC-B8-ED-F3-4B-F2-06-C6-74-27-EC-2A-82-EC-50-0F-63-8C-CB-CE-32-79-38-F1-AC-83-43-C8-D7-96-4E", id);
        }

        [TestMethod]
        public void TestMethod8()
        {
            // Elapsed is a monotonic increasing function over time

            var context = new OperationContext();

            var t = context.Elapsed;
            for (int i = 0; i < 10; i++)
            {
                Thread.Yield();
                Assert.IsTrue(t < context.Elapsed);
            }
        }

        [TestMethod]
        public void TestMethod9()
        {
            // Remaining is a monotonic decreasing function over time

            var context = new OperationContext();

            var t = context.Remaining;
            for (int i = 0; i < 10; i++)
            {
                Thread.Yield();
                Assert.IsTrue(context.Remaining < t);
            }
        }
    }
}
