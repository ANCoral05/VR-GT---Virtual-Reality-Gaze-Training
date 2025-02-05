using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRK_BuildingBlocks
{
    public class ClickInteractionComponent : MonoBehaviour, IInteractionComponent
    {
        public UnityEvent OnHoverStart;
        public UnityEvent OnHoverEnd;

        private List<IControllerComponent> hoveringControllers = new List<IControllerComponent>();

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
