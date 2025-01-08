using System.Collections.Generic;
using UnityEngine;

namespace VRK_BuildingBlocks
{
    public class EntityMovementScript : MonoBehaviour
    {
        public enum TransformationMethod
        {
            StaticTransformation,
            TargetKeys,
            RandomizedTransformation
        }

        public TransformationMethod method = TransformationMethod.StaticTransformation;
        
        [Header("Movement")]
        [Tooltip("Speed which the entity aims to move at.")]
        public float targetSpeed = 1;

        [Tooltip("The acceleration of the entity. Set to -1 for infinite acceleration (instant targeted speed).")]
        public float movementAcceleration = -1;

        [Tooltip("The deceleration of the entity. Set to -1 for infinite deceleration (instant breaks).")]
        public float movementDampening = -1;

        [Tooltip("The amount of 'bounce' or oscillation when stopping movement. 0 means no oscillation, 1 means endless oscillation.")]
        public float movementOscillation = 0;

        public float currentSpeed { get; private set; }
        public Vector3 currentDirection { get; private set; }

        [SerializeField, Tooltip("If activated, the target will face in moving direction.")]
        private bool faceTowardsMovementDirection = false;

        [Header("Rotation")]
        [Tooltip("Speed which the entity aims to rotate at.")]
        public float targetRotationSpeed = 1;

        [Tooltip("The angular acceleration of the entity. Set to -1 for infinite acceleration (instant targeted speed).")]
        public float rotationAcceleration = -1;

        [Tooltip("The angular deceleration of the entity. Set to -1 for infinite deceleration (instant breaks).")]
        public float rotationDampening = -1;

        [Tooltip("The amount of rotational 'bounce' or oscillation when stopping movement. 0 means no oscillation, 1 means endless oscillation.")]    
        public float rotationOscillation;
        public float currentRotationSpeed { get; private set; }

        [Header("Scale")]
        [Tooltip("Speed which the entity aims to scale at.")]
        public float targetScaleSpeed = -1;
        public float currentScaleSpeed { get; private set; }

        [Header("Static Transform")]
        [Tooltip("Targeted direction of the movement.")]
        public Vector3 targetDirection = Vector3.forward;

        [Header("Target Key Transforms")]
        [Tooltip("The keys towards which the entity will move.")]
        public List<Transform> targetKeys = new List<Transform>();



        public void StaticTransformation()
        {
            this.transform.position += currentDirection * targetSpeed * Time.deltaTime;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
