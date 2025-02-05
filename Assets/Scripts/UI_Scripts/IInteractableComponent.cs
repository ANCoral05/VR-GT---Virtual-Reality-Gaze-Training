using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRK_BuildingBlocks
{
    public interface IInteractionComponent
    {
        void OnConditionStart(IControllerComponent controller);
        void OnConditionEnd(IControllerComponent controller);
    }
}
