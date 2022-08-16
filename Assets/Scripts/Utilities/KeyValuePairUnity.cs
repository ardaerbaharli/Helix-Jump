using System;

namespace Utilities
{
    [Serializable]
    public class KeyValuePairUnity<K, V>
    {
        public K key;
        public V value;

        public KeyValuePairUnity(K key, V value)
        {
            this.key = key;
            this.value = value;
        }
    }
}