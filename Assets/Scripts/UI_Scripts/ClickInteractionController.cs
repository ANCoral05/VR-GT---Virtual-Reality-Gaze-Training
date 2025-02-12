using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRK_BuildingBlocks
{
    public class ClickInteractionController : MonoBehaviour, IControllerComponent
    {
        [SerializeField] private Transform raycastOrigin;

        [SerializeField] private GameObject raycastVisualizer;

        [SerializeField] private LayerMask layerMask;

        public bool isActiveController;

        private Transform raycastTarget;

        private void Update()
        {
            if (isActiveController)
            {
                GetRaycastTarget();
            }
        }

        public void SetControllerActiveState(bool isActive)
        {
            isActiveController = isActive;

            raycastVisualizer.SetActive(isActive);
        }

        private void GetRaycastTarget()
        {
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out RaycastHit hit, 100, layerMask))
            {
                if(raycastTarget != hit.transform)
                {
                    CallInteractionMethods(hit.transform, raycastTarget);

                    raycastTarget = hit.transform;
                }
            }
            else
            {

                if (raycastTarget != null)
                {
                    CallInteractionMethods(null, raycastTarget);

                    raycastTarget = null;
                }
            }
        }

        private void CallInteractionMethods(Transform newTarget, Transform previousTarget)
        {
            if (newTarget != null)
            {
                if (newTarget.TryGetComponent(out IInteractionCondition interactionComponent))
                {
                    interactionComponent.OnConditionStart(this);
                }
            }
            
            if (previousTarget != null)
            {
                if (previousTarget.TryGetComponent(out IInteractionCondition interactionComponent))
                {
                    interactionComponent.OnConditionEnd(this);
                }
            }

        }
    }
}
