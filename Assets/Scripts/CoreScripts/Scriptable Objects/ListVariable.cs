using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRK_BuildingBlocks
{
    [CreateAssetMenu(fileName = "ListVariable", menuName = "ScriptableObjectVariables/List Variable")]
    public class ListVariable<T> : ScriptableObjectVariable<List<T>>, IList<T>
    {
        public event Action OnValueChange;
        public event Action<string> OnValueChangeDetailed;

        [SerializeField]
        private List<T> list = new List<T>();

        public T this[int index]
        {
            get => list[index];
            set
            {
                list[index] = value;
                TriggerChange($"Set index {index} to {value}");
            }
        }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            list.Add(item);
            TriggerChange($"Added {item}");
        }

        public void Clear()
        {
            list.Clear();
            TriggerChange("Cleared list");
        }

        public bool Contains(T item) => list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        public int IndexOf(T item) => list.IndexOf(item);

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            TriggerChange($"Inserted {item} at index {index}");
        }

        public bool Remove(T item)
        {
            bool removed = list.Remove(item);
            if (removed)
            {
                TriggerChange($"Removed {item}");
            }
            return removed;
        }

        public void RemoveAt(int index)
        {
            T removedItem = list[index];
            list.RemoveAt(index);
            TriggerChange($"Removed {removedItem} from index {index}");
        }

        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        private void TriggerChange(string detail = null)
        {
            OnValueChange?.Invoke();
            if (!string.IsNullOrEmpty(detail))
            {
                OnValueChangeDetailed?.Invoke(detail);
            }
        }
    }
}