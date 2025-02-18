using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VRK_BuildingBlocks
{
    public abstract class ListVariable<T> : ScriptableObjectVariable<List<T>>, IList<T>
    {
        public event Action OnValueChange;
        public event Action<string> OnValueChangeDetailed;

        public T this[int index]
        {
            get => Value[index];
            set
            {
                Value[index] = value;
                TriggerChange($"Set index {index} to {value}");
            }
        }

        public int Count => Value.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            Value.Add(item);
            TriggerChange($"Added {item}");
        }

        public void Clear()
        {
            Value.Clear();
            TriggerChange("Cleared list");
        }

        public bool Contains(T item) => Value.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => Value.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => Value.GetEnumerator();

        public int IndexOf(T item) => Value.IndexOf(item);

        public void Insert(int index, T item)
        {
            Value.Insert(index, item);
            TriggerChange($"Inserted {item} at index {index}");
        }

        public bool Remove(T item)
        {
            bool removed = Value.Remove(item);
            if (removed)
            {
                TriggerChange($"Removed {item}");
            }
            return removed;
        }

        public void RemoveAt(int index)
        {
            T removedItem = Value[index];
            Value.RemoveAt(index);
            TriggerChange($"Removed {removedItem} from index {index}");
        }

        IEnumerator IEnumerable.GetEnumerator() => Value.GetEnumerator();

        private void TriggerChange(string detail = null)
        {
            OnValueChange?.Invoke();
            if (!string.IsNullOrEmpty(detail))
            {
                OnValueChangeDetailed?.Invoke(detail);
            }
        }

        protected override void Reset()
        {
            Value = new List<T>(resetValue);

            if (showChangesInDebugLog)
            {
                Debug.Log($"List Variable '{name}' reset.");
            }
        }

        protected override void SetResetValue()
        {
            if (!Value.SequenceEqual(resetValue))
            {
                resetValue = new List<T>(Value);
            }

            Debug.Log($"List Variable '{name}' reset value was updated.");
        }
    }
}