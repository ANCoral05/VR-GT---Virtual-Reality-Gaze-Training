using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace VRK_BuildingBlocks
{
    public class ClickInteractionComponent : MonoBehaviour, IInteractionCondition
    {
        public UnityEvent OnHoverStart;
        public UnityEvent OnHoverEnd;

        [HideInInspector] public bool ConditionActive => hoveringControllers.Count > 0;

        [SerializeField] private List<InputAction> inputActions = new List<InputAction>();

        private List<IControllerComponent> hoveringControllers = new List<IControllerComponent>();

        private void OnEnable()
        {
            foreach (InputAction inputAction in inputActions)
            {
                // create a Unity
            }
        }

        public void OnConditionStart(IControllerComponent controller)
        {
            OnHoverStart.Invoke();

            if (!hoveringControllers.Contains(controller))
            {
                hoveringControllers.Add(controller);
            }
        }
        public void OnConditionEnd(IControllerComponent controller)
        {
            OnHoverEnd.Invoke();

            if (hoveringControllers.Contains(controller))
            {
                hoveringControllers.Remove(controller);
            }
        }
    }
}
