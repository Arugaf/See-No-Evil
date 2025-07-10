using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Registries
{
    public class IdentifiableRegistry<T> : ScriptableObject, IRegistry<T> where T : IdentifiableScriptableObject
    {
        [field: SerializeField] public T[] Values { get; private set; }
        private Dictionary<string, T> keyValuePairs;
        public T Get(string id)
        {
            return Ensure().GetValueOrDefault(id);
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return Ensure().GetEnumerator();
        }

        private Dictionary<string, T> Ensure()
        {
            if (keyValuePairs == null)
            {
                keyValuePairs = new Dictionary<string, T>();
                foreach (var x in Values)
                {
                    keyValuePairs.Add(x.ID, x);
                }
            }
            return keyValuePairs;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}