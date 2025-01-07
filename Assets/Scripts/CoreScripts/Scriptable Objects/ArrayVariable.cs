using UnityEngine;

namespace VRK_BuildingBlocks
{
    [CreateAssetMenu(menuName = "Variables/Array Variable")]
    public class ArrayVariable<T> : ScriptableObject
    {
        [SerializeField]
        private T[] array;

        public T[] Array
        {
            get => array;
            set => array = value;
        }

        public int Length => array.Length;

        public T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public void SetArray(T[] newArray)
        {
            array = newArray;
        }

        public void SetElement(int index, T element)
        {
            array[index] = element;
        }

        public T GetElement(int index)
        {
            return array[index];
        }
    }
}
