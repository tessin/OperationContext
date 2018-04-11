using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Tessin.Diagnostics.Internal
{
    [JsonConverter(typeof(OperationValueDictionaryJsonConverter))]
    public class OperationValueDictionary : IEnumerable<KeyValuePair<OperationValueKey, object>>
    {
        public static readonly OperationValueDictionary Empty = new OperationValueDictionary();

        private OperationValueDictionary _parent;
        private KeyValuePair<OperationValueKey, object> _item;

        public bool IsEmpty => _parent == null;
        public bool HasValue => _parent != null;

        private OperationValueDictionary()
        {
        }

        private OperationValueDictionary(OperationValueDictionary parent, KeyValuePair<OperationValueKey, object> item)
        {
            this._parent = parent;
            this._item = item;
        }

        public object GetItem(OperationValueKey key)
        {
            var next = this;
            while (next.HasValue)
            {
                if (next._item.Key.Equals(key))
                {
                    return next._item.Value;
                }
            }
            return null;
        }

        public OperationValueDictionary SetItem(OperationValueKey key, object value)
        {
            return new OperationValueDictionary(this, new KeyValuePair<OperationValueKey, object>(key, value));
        }

        public IEnumerator<KeyValuePair<OperationValueKey, object>> GetEnumerator()
        {
            // the set of keys will be few
            var ks = new HashSet<OperationValueKey>();

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
