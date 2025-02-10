using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRK_BuildingBlocks
{
    public interface IInteractionCondition
    {
        void OnConditionStart(IControllerComponent controller);
        void OnConditionEnd(IControllerComponent controller);
        bool ConditionActive { get; }
    }
}
