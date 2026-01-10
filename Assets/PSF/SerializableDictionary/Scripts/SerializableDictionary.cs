using System;
using System.Collections.Generic;
using UnityEngine;

namespace SerializableDictionary.Scripts
{
    [Serializable] 
    public struct KeyValuePair<TKey, TValue>
    {
        [SerializeField] private TKey _key;
        [SerializeField] private TValue _value;
        public TKey Key => _key;
        public TValue Value => _value;
        
        public KeyValuePair(System.Collections.Generic.KeyValuePair<TKey, TValue> pair)
        {
            _key = pair.Key;
            _value = pair.Value;
        }
    }

    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        [SerializeField] private KeyValuePair<TKey, TValue>[] _pairs;
        private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        public Dictionary<TKey, TValue> Dictionary
        {
            get
            {
                return _dictionary;
            }
        }

        public int Count => Dictionary.Count;
        public void Add(TKey key, TValue value) {
            _dictionary.Add(key, value);
            this._pairs = GenerateSerializableArray();
        }
        public bool Remove(TKey key) {
            bool result = _dictionary.Remove(key);
            this._pairs = GenerateSerializableArray();
            return result;
        }
        public void Clear() {
            _dictionary.Clear();
            this._pairs = GenerateSerializableArray();
        }
        public bool ContainsKey(TKey key) {
            return _dictionary.ContainsKey(key);
        }
        public bool ContainsValue(TValue value) => Dictionary.ContainsValue(value);
        public TValue Get(TKey key) => Dictionary[key];
        public Dictionary<TKey, TValue>.KeyCollection Keys() => Dictionary.Keys;
        public Dictionary<TKey, TValue>.ValueCollection Values() => Dictionary.Values;
        public void Set(TKey key, TValue value) {
            _dictionary[key] = value;
            this._pairs = GenerateSerializableArray();
        }
        public Dictionary<TKey, TValue>.Enumerator GetEnumerator() => Dictionary.GetEnumerator();

        public KeyValuePair<TKey, TValue>[] GenerateSerializableArray()
        {
            KeyValuePair<TKey, TValue>[] pairs = new KeyValuePair<TKey, TValue>[Dictionary.Count];
            Dictionary<TKey, TValue>.Enumerator enumerator = Dictionary.GetEnumerator();

            int index = 0;
            while (enumerator.MoveNext())
            {
                pairs[index] = new KeyValuePair<TKey, TValue>(enumerator.Current);
                index++;
            }
            
            return pairs;
        }
    }
}