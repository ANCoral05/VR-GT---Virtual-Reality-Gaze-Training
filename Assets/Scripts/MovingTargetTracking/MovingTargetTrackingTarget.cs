using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRK_BuildingBlocks
{
    public class MovingTargetTrackingTarget : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable origin;

        private EntityMovementScript entityMovementScript;

        private void Start()
        {
            if (GetComponent<EntityMovementScript>() != null)
            {
                entityMovementScript = GetComponent<EntityMovementScript>();
            }
        }

        private void Update()
        {
            if(entityMovementScript.randomAreaOrigin == null)
            {
                if(entityMovementScript == null || origin.Value == null)
                {
                    return;
                }

                entityMovementScript.randomAreaOrigin = origin.Value.transform;
            }
        }
    }
}
