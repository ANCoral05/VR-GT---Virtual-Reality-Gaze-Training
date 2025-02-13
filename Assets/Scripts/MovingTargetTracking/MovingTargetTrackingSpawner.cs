using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRK_BuildingBlocks
{
    public class MovingTargetTrackingSpawner : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable origin;

        private void OnEnable()
        {
            origin.Value = this.gameObject;
        }
    }
}
