using UnityEngine;

namespace VRK_BuildingBlocks
{
    [CreateAssetMenu(menuName = "Variables/Int Variable")]
    public class IntVariable : ScriptableObject
    {
        public int value { get; set; }
    }
}
