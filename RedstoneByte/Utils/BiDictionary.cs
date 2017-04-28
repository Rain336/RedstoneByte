using System.Collections;
using System.Collections.Generic;

namespace RedstoneByte.Utils
{
    public sealed class BiDictionary<TK, TV> : IDictionary<TK, TV>
    {
        private readonly IDictionary<TK, TV> _internal = new Dictionary<TK, TV>();
        private readonly IDictionary<TV, TK> _reversed = new Dictionary<TV, TK>();

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            return _internal.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internal.GetEnumerator();
        }

        public void Add(KeyValuePair<TK, TV> item)
        {
            _internal.Add(item);
            _reversed.Add(item.Value, item.Key);
        }

        public void Add(KeyValuePair<TV, TK> item)
        {
            _reversed.Add(item);
            _internal.Add(item.Value, item.Key);
        }

        public bool Contains(KeyValuePair<TV, TK> item)
        {
            return _reversed.Contains(item);
        }

        public void CopyTo(KeyValuePair<TV, TK>[] array, int arrayIndex)
        {
            _reversed.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TV, TK> item)
        {
            _internal.Remove(item.Value);
            return _reversed.Remove(item);
        }

        public void Clear()
        {
            _internal.Clear();
            _reversed.Clear();
        }

        public bool Contains(KeyValuePair<TK, TV> item)
        {
            return _internal.Contains(item);
        }

        public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex)
        {
            _internal.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TK, TV> item)
        {
            _reversed.Remove(item.Value);
            return _internal.Remove(item);
        }

        public int Count => _internal.Count;

        public bool IsReadOnly => _internal.IsReadOnly;

        public void Add(TK key, TV value)
        {
            _internal.Add(key, value);
            _reversed.Add(value, key);
        }

        public bool ContainsKey(TK key)
        {
            return _internal.ContainsKey(key);
        }

        public bool Remove(TK key)
        {
            _reversed.Remove(_internal[key]);
            return _internal.Remove(key);
        }

        public bool TryGetValue(TK key, out TV value)
        {
            return _internal.TryGetValue(key, out value);
        }

        public TV this[TK key]
        {
            get => _internal[key];
            set
            {
                _internal[key] = value;
                _reversed[value] = key;
            }
        }

        public TK this[TV key]
        {
            get => _reversed[key];
            set
            {
                _reversed[key] = value;
                _internal[value] = key;
            }
        }

        public void Add(TV key, TK value)
        {
            _reversed.Add(key, value);
            _internal.Add(value, key);
        }

        public bool ContainsKey(TV key)
        {
            return _reversed.ContainsKey(key);
        }

        public bool Remove(TV key)
        {
            _internal.Remove(_reversed[key]);
            return _reversed.Remove(key);
        }

        public bool TryGetValue(TV key, out TK value)
        {
            return _reversed.TryGetValue(key, out value);
        }

        public ICollection<TK> Keys => _internal.Keys;

        public ICollection<TV> Values => _internal.Values;
    }
}