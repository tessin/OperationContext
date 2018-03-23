using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Tessin.Diagnostics.Internal;

namespace Tessin.Diagnostics
{
    [JsonConverter(typeof(OperationValueDictionaryJsonConverter))]
    public class OperationValueDictionary : IEnumerable<KeyValuePair<string, OperationValue>>
    {
        public static readonly OperationValueDictionary Empty = new OperationValueDictionary();

        private OperationValueDictionary _parent;
        private KeyValuePair<string, OperationValue> _item;

        public bool IsEmpty => _parent == null;
        public bool HasValue => _parent != null;

        private OperationValueDictionary()
        {
        }

        private OperationValueDictionary(OperationValueDictionary parent, KeyValuePair<string, OperationValue> item)
        {
            this._parent = parent;
            this._item = item;
        }

        public OperationValueDictionary SetItem(string key, OperationValue value)
        {
            return new OperationValueDictionary(this, new KeyValuePair<string, OperationValue>(key, value));
        }

        public IEnumerator<KeyValuePair<string, OperationValue>> GetEnumerator()
        {
            // the set of keys will be few
            var ks = new HashSet<string>(StringComparer.Ordinal);

            var next = this;
            while (next.HasValue)
            {
                var curr = next;
                next = next._parent;
                var item = curr._item;
                if (ks.Add(item.Key)) // a more recent version of a value with the same key will simply shadow that
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
