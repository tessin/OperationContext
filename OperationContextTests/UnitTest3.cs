using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Tessin.Diagnostics.Internal;

namespace Tessin.Diagnostics
{
    [TestClass]
    public class UnitTest3
    {
        enum Key
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
        }

        [TestMethod, Description("immutable dictionary")]
        public void OperationValueDictionary_Count_Test1()
        {
            Assert.AreEqual(0, OperationValueDictionary.Empty.Count());
        }

        [TestMethod, Description("immutable dictionary")]
        public void OperationValueDictionary_Count_Test2()
        {
            var dict = OperationValueDictionary.Empty;

            dict = dict.SetItem(OperationValueKey.Create(Key.A), 1);

            Assert.AreEqual(1, dict.Count());
        }

        [TestMethod, Description("immutable dictionary")]
        public void OperationValueDictionary_Count_Test3()
        {
            var dict = OperationValueDictionary.Empty;

            dict = dict.SetItem(OperationValueKey.Create(Key.A), 1);
            dict = dict.SetItem(OperationValueKey.Create(Key.A), 2);
            dict = dict.SetItem(OperationValueKey.Create(Key.A), 3);

            Assert.AreEqual(1, dict.Count());
        }

        [TestMethod, Description("immutable dictionary")]
        public void OperationValueDictionary_SetItem_Test1()
        {
            var dict = OperationValueDictionary.Empty;

            dict = dict.SetItem(OperationValueKey.Create(Key.A), 1);

            CollectionAssert.Contains(dict.ToList(), new KeyValuePair<OperationValueKey, object>(OperationValueKey.Create(Key.A), 1));
        }

        [TestMethod, Description("immutable dictionary")]
        public void OperationValueDictionary_SetItem_Test2()
        {
            var dict = OperationValueDictionary.Empty;

            dict = dict.SetItem(OperationValueKey.Create(Key.A), 1);
            dict = dict.SetItem(OperationValueKey.Create(Key.A), 2);
            dict = dict.SetItem(OperationValueKey.Create(Key.A), 3);

            CollectionAssert.Contains(dict.ToList(), new KeyValuePair<OperationValueKey, object>(OperationValueKey.Create(Key.A), 3));
        }

        [TestMethod, Description("immutable dictionary")]
        public void OperationValueDictionary_SetItem_Test3()
        {
            var xs = new KeyValuePair<OperationValueKey, object>[10];

            for (int i = 0; i < xs.Length; i++)
            {
                var k = OperationValueKey.Create((Key)((int)Key.A + i));

                xs[i] = new KeyValuePair<OperationValueKey, object>(k, i + 1);
            }

            for (int k = 0; k < 1000; k++)
            {
                var dict1 = new Dictionary<OperationValueKey, object>();
                var dict2 = OperationValueDictionary.Empty;

                var r = new Random(3);

                for (int i = 0, n = r.Next(xs.Length) + 1; i < n; i++)
                {
                    var x = xs[r.Next(xs.Length)];

                    dict1[x.Key] = x.Value;
                    dict2 = dict2.SetItem(x.Key, x.Value);
                }

                System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(dict1));
                System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(dict2));

                CollectionAssert.AreEquivalent(dict1.ToList(), dict2.ToList());
            }
        }
    }
}
