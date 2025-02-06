using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace VRK_BuildingBlocks
{
    public class InpuActionEventComponent : MonoBehaviour
    {
        [SerializeField] private InputAction inputAction;

        [SerializeField] private bool ifHovered = true;

        [SerializeField] private bool ifGrabbed;

        [SerializeField] private bool ifGazeTarget;

        [SerializeField] private UnityEvent onActionStart;

        [SerializeField] private UnityEvent onActionEnd;

        public void OnInputStart()
        {
            if (ifHovered)
            {
                TryGetComponent<ClickInteractionComponent>(out ClickInteractionComponent clickInteractionComponent);

                if (clickInteractionComponent != null)
                {
                    if (clickInteractionComponent.ConditionActive)
                    {
                        onActionStart.Invoke();
                    }
                }
                else
                {
                    Debug.LogError("ClickInteractionComponent not found");
                }
            }

            // repeat for ifGrabbed and ifGazeTarget
            if (ifGrabbed)
            {
                TryGetComponent<ProximityInteractionComponent>(out ProximityInteractionComponent proximityInteractionComponent);

                if (proximityInteractionComponent != null)
                {
                    if (proximityInteractionComponent.ConditionActive)
                    {
                        onActionStart.Invoke();
                    }
                }
                else
                {
                    Debug.LogError("GrabInteractionComponent not found");
                }

                onActionStart.Invoke();
            }
        }

        public void OnInputEnd()
        {
            onActionEnd.Invoke();
        }
    }
}
