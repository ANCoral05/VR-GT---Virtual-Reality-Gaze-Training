using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace VRK_BuildingBlocks
{
    public class ProximityInteractionComponent : MonoBehaviour, IInteractionCondition
    {
        public UnityEvent OnDistanceReached;
        public UnityEvent OnDistanceLeft;

        [HideInInspector] public bool ConditionActive => ControllersInProximity.Count > 0;

        [SerializeField] private List<InputAction> inputActions = new List<InputAction>();

        private List<IControllerComponent> ControllersInProximity = new List<IControllerComponent>();

        public void OnConditionStart(IControllerComponent controller)
        {
            OnDistanceReached.Invoke();

            if (!ControllersInProximity.Contains(controller))
            {
                ControllersInProximity.Add(controller);
            }
        }
        public void OnConditionEnd(IControllerComponent controller)
        {
            OnDistanceLeft.Invoke();

            if (ControllersInProximity.Contains(controller))
            {
                ControllersInProximity.Remove(controller);
            }
        }
    }
}
