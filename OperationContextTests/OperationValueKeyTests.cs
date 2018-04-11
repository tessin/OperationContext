using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Tessin.Diagnostics.Internal;

namespace Tessin.Diagnostics
{
    [TestClass, TestCategory("OperationValueKey")]
    public class OperationValueKeyTests
    {
        enum A
        {
            A,
            B,
            C,
        }

        enum B
        {
            [EnumMember(Value = "X")]
            A,
            [EnumMember(Value = "Y")]
            B,
            [EnumMember(Value = "Z")]
            C,
        }

        [TestMethod]
        public void OperationValueKey_Equality_Test()
        {
            var a = OperationValueKey.Create(A.A);
            var b = OperationValueKey.Create(B.A);

            Assert.AreEqual(a, a);
            Assert.AreEqual(b, b);

            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, a);
        }

        [TestMethod]
        public void OperationValueKey_ToString_Test()
        {
            Assert.AreEqual("A", OperationValueKey.Create(A.A).ToString());
            Assert.AreEqual("B", OperationValueKey.Create(A.B).ToString());
            Assert.AreEqual("C", OperationValueKey.Create(A.C).ToString());

            Assert.AreEqual("X", OperationValueKey.Create(B.A).ToString());
            Assert.AreEqual("Y", OperationValueKey.Create(B.B).ToString());
            Assert.AreEqual("Z", OperationValueKey.Create(B.C).ToString());
        }

        [TestMethod]
        public void OperationValueKey_Serialize_Test()
        {
            Assert.AreEqual("\"A\"", JsonConvert.SerializeObject(OperationValueKey.Create(A.A)));
            Assert.AreEqual("\"B\"", JsonConvert.SerializeObject(OperationValueKey.Create(A.B)));
            Assert.AreEqual("\"C\"", JsonConvert.SerializeObject(OperationValueKey.Create(A.C)));

            Assert.AreEqual("\"X\"", JsonConvert.SerializeObject(OperationValueKey.Create(B.A)));
            Assert.AreEqual("\"Y\"", JsonConvert.SerializeObject(OperationValueKey.Create(B.B)));
            Assert.AreEqual("\"Z\"", JsonConvert.SerializeObject(OperationValueKey.Create(B.C)));
        }
    }
}
