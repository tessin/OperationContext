using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tessin.Diagnostics
{
    [TestClass]
    public class UnitTest1
    {
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
            var x = 3;
            context = context.WithState(nameof(x), 3);
            CollectionAssert.Contains(context.State.ToList(), new KeyValuePair<string, OperationValue>("x", 3));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var context = new OperationContext();
            var x = 3;
            context = context.WithState(nameof(x), 3).WithState("y", 5);
            CollectionAssert.Contains(context.State.ToList(), new KeyValuePair<string, OperationValue>("y", 5));
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
            context = context.WithState(nameof(i), i);

            var json = JsonConvert.SerializeObject(context);

            var arr = JArray.Parse(json);

            Assert.AreEqual(i, (int)arr[0]["s"]["i"]);

            var id = BitConverter.ToString(OperationContext.Id(OperationContext.Backtrace(context)));

            Assert.AreEqual("43-4C-24-65-9F-DE-C0-62-A1-93-AB-E1-5D-C8-21-E4-62-C4-08-BD-65-EB-2B-01-EE-CD-BF-C6-A7-65-C2-7D", id);
        }
    }
}
