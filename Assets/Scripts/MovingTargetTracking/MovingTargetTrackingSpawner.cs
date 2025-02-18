using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRK_BuildingBlocks
{
    public class MovingTargetTrackingSpawner : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable origin;

        private void Awake()
        {
            if (origin.Value == null)
            {
                origin.Value = this.gameObject;
            }
        }

        private void OnEnable()
        {
            if (origin.Value == null)
            {
                origin.Value = this.gameObject;
            }
        }
    }
}
