using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeQuestUtils
{
    // Enum for controller keys
    public enum GazeQuestKey
    {
        Trigger,
        Primary,
        Secondary,
        Handle,
        ThumbstickPress
    }

    // Enum for controller hands
    public enum ControllerHand
    {
        Left,
        Right
    }

    public class ControllerKey
    {
        // Properties to hold the key and the controller hand
        public GazeQuestKey Key { get; set; }
        public ControllerHand Hand { get; set; }

        // Constructor
        public ControllerKey(GazeQuestKey key, ControllerHand hand)
        {
            Key = key;
            Hand = hand;
        }
    }
}
