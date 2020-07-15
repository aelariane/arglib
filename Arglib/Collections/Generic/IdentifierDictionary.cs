using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Arglib.Collections.Generic
{
    public class IdentifierDictionary<TValue> : IDictionary<OptionIdentifier, TValue>
    {
        private readonly Dictionary<char, TValue> _charDictionary;
        private readonly OptionIdentifierComparer _comparer = OptionIdentifierComparer.Instance;
        private ulong _generation = 0;
        private readonly Dictionary<string, TValue> _stringDictionary;
        private readonly List<OptionIdentifier> _uniqueKeys = new List<OptionIdentifier>();

        public int Count => _uniqueKeys.Count;

        public bool IsReadOnly => false;

        public ICollection<OptionIdentifier> Keys => new List<OptionIdentifier>(_uniqueKeys);

        public ICollection<TValue> Values => new List<TValue>(_charDictionary.Values.Union(_stringDictionary.Values));

        public TValue this[OptionIdentifier key]
        {
            get
            {
                TValue result;
                TryGetValue(key, out result);
                return result;
            }
            set
            {
                _generation++;
                OptionIdentifier fullKey;
                if (TryFindKey(key, out fullKey))
                {
                    UpdateValue(key.Combine(fullKey), value);
                    UpdateKeys(key);
                    return;
                }
                Add(key, value);
            }
        }

        public IdentifierDictionary()
        {
            _charDictionary = new Dictionary<char, TValue>();
            _stringDictionary = new Dictionary<string, TValue>();
        }

        public IdentifierDictionary(int capacity)
        {
            _charDictionary = new Dictionary<char, TValue>(capacity);
            _stringDictionary = new Dictionary<string, TValue>(capacity);
        }

        public void Add(OptionIdentifier key, TValue value)
        {
            _generation++;
            bool exists = TryFindKey(key, out OptionIdentifier exKey);

            if (exists)
            {
                throw new InvalidOperationException("The key already exists");
            }

            if (key.HasAlias)
            {
                _charDictionary.Add(key.Alias, value);
            }
            if (key.HasName)
            {
                _stringDictionary.Add(key.Name, value);
            }

            _uniqueKeys.Add(new OptionIdentifier(
                key.Alias,
                key.Name
            ));
        }

        public void Add(KeyValuePair<OptionIdentifier, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _charDictionary.Clear();
            _stringDictionary.Clear();
            _uniqueKeys.Clear();
            _generation = 0;
        }

        public bool Contains(KeyValuePair<OptionIdentifier, TValue> item)
        {
            if(TryGetValue(item.Key, out TValue checkItem))
            {
                return checkItem.Equals(item);
            }
            return false;
        }

        public bool ContainsKey(OptionIdentifier key)
        {
            return
                (key.HasAlias && _charDictionary.ContainsKey(key.Alias)) ||
                (key.HasName && _stringDictionary.ContainsKey(key.Name));
        }

        public void CopyTo(KeyValuePair<OptionIdentifier, TValue>[] array, int arrayIndex)
        {
            foreach(var pair in this)
            {
                array[arrayIndex++] = pair;
            }
        }

        public IEnumerator<KeyValuePair<OptionIdentifier, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        public bool Remove(OptionIdentifier key)
        {
            _generation++;
            bool result = (key.HasAlias && _charDictionary.Remove(key.Alias)) |
                (key.HasName && _stringDictionary.Remove(key.Name));

            if (result)
            {
                RemoveKey(key);
            }

            return result;
        }

        public bool Remove(KeyValuePair<OptionIdentifier, TValue> item)
        {
            _generation++;
            OptionIdentifier key = item.Key;

            bool exists = TryFindKey(key, out OptionIdentifier exKey);

            if (exists)
            {
                RemoveKey(key);
                return
                    (exKey.HasAlias && _charDictionary.Remove(exKey.Alias)) |
                    (exKey.HasName && _stringDictionary.Remove(exKey.Name));
            }

            return false;
        }

        private void RemoveKey(in OptionIdentifier key)
        {
            for(int i = 0; i < _uniqueKeys.Count; i++)
            {
                if (_uniqueKeys[i].CompareTo(key) > 0)
                {
                    _uniqueKeys.RemoveAt(i);
                    return;
                }
            }
        }

        private bool TryFindKey(in OptionIdentifier key, out OptionIdentifier result)
        {
            for(int i = 0; i < _uniqueKeys.Count; i++)
            {
                if (_comparer.Compare(key, _uniqueKeys[i]) > 0)
                {
                    result = _uniqueKeys[i];
                    return true;
                }
            }

            result = default;
            return false;
        }

        public bool TryGetValue(OptionIdentifier key, out TValue value)
        {
            value = default;
            return 
                (key.HasAlias && _charDictionary.TryGetValue(key.Alias, out value)) ||
                (key.HasName && _stringDictionary.TryGetValue(key.Name, out value));
        }

        private void UpdateKeys(in OptionIdentifier identifier)
        {
            int index = -1;

            for(int i = 0; i < _uniqueKeys.Count; i++)
            {
                if (_uniqueKeys[i].OneMatch(identifier))
                {
                    index = i;
                    break;
                }
            }

            if(index == -1)
            {
                _uniqueKeys.Add(identifier);
            }
            else
            {
                _uniqueKeys[index] = _uniqueKeys[index].Combine(identifier);
            }
        }

        private void UpdateValue(in OptionIdentifier key, TValue value)
        {
            if (key.HasAlias)
            {
                _charDictionary[key.Alias] = value;
            }
            if (key.HasName)
            {
                _stringDictionary[key.Name] = value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private struct Enumerator : IEnumerator<KeyValuePair<OptionIdentifier, TValue>>
        {
            private OptionIdentifier _currentKey;
            private readonly ulong _generation;
            private readonly int _maxId;
            private int _index;
            private readonly IdentifierDictionary<TValue> _reference;

            public KeyValuePair<OptionIdentifier, TValue> Current =>
                new KeyValuePair<OptionIdentifier, TValue>(
                    _currentKey,
                    _reference[_currentKey]
                );

            object IEnumerator.Current => Current;

            internal Enumerator(IdentifierDictionary<TValue> parent)
            {
                _currentKey = new OptionIdentifier();
                _reference = parent;
                _index = -1;
                _generation = parent._generation;
                _maxId = parent._uniqueKeys.Count;
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                if(_reference._generation != _generation)
                {
                    throw new InvalidOperationException("Dictionary was modified");
                }
                bool result = ++_index < _maxId;
                if (result)
                {
                    _currentKey = _reference._uniqueKeys[_index];
                }
                return result;
            }

            public void Reset()
            {
                _index = -1;
            }
        }
    }
}
