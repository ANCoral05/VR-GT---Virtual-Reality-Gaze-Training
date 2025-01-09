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

        public bool movementActive = true;

        public TransformationMethod movementMethod = TransformationMethod.StaticTransformation;
        
        [Header("Movement")]
        [Tooltip("Speed which the entity aims to move at.")]
        public float targetSpeed = 1;

        [Tooltip("The acceleration of the entity. Set to -1 for infinite acceleration (instant targeted speed).")]
        public float movementAcceleration = -1;

        [Tooltip("The deceleration of the entity. Set to -1 for infinite deceleration (instant breaks).")]
        public float movementDampening = -1;

        [Tooltip("Maximum turn speed (in degree per second) of the entity. Set to -1 for infinite turn speed.")]
        public float maxTurnSpeed = -1;

        [Tooltip("The amount of 'bounce' or oscillation when stopping movement. 0 means no oscillation, 1 means endless oscillation.")]
        public float movementOscillation = 0;

        public float currentSpeed { get; private set; }

        public Vector3 currentDirection { get; private set; }

        [SerializeField, Tooltip("If activated, the target will face in moving direction.")]
        private bool faceTowardsMovementDirection = false;

        [Header("Static Transform")]
        [Tooltip("Targeted direction of the movement.")]
        public Vector3 targetDirection = Vector3.forward;

        [Header("Target Key Transforms")]
        [Tooltip("The keys towards which the entity will move.")]
        public List<Transform> targetKeys = new List<Transform>();

        private int currentKeyIndex = 0;


        public void StaticTransformationMovement()
        {
            this.transform.position += currentDirection * currentSpeed * Time.deltaTime;
        }

        public void TargetKeysMovement()
        {
            if (targetKeys.Count == 0)
            {
                return;
            }

            Vector3 targetPosition = targetKeys[currentKeyIndex].position;

            targetDirection = targetPosition - this.transform.position;

            // if the target is reached, move to the next target
            float distance = (targetPosition - this.transform.position).magnitude;

            if (distance < 0.01f)
            {
                this.transform.position = targetPosition;
                currentKeyIndex = (currentKeyIndex + 1) % targetKeys.Count;
            }
        }

        public void CurrentSpeedAndDirection()
        {
            if (currentSpeed < targetSpeed)
            {
                if (movementAcceleration < 0)
                {
                    currentSpeed = targetSpeed;
                }
                else
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, movementAcceleration * Time.deltaTime);
                }
            }

            else if (currentSpeed > targetSpeed)
            {
                if (movementDampening < 0)
                {
                    currentSpeed = targetSpeed;
                }
                else
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, movementDampening * Time.deltaTime);
                }
            }

            if (currentDirection != targetDirection)
            {
                currentDirection = Vector3.RotateTowards(currentDirection, targetDirection, maxTurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 0);
            }

            // move the entity
            this.transform.position += currentDirection * currentSpeed * Time.deltaTime;
        }

        // function to face the entity towards the movement direction
        public void FaceTowardsMovementDirection()
        {
            if (faceTowardsMovementDirection)
            {
                this.transform.rotation = Quaternion.LookRotation(currentDirection);
            }
        }

        public void RandomizedTransformationMovement()
        {
            // TODO
        }

        // Update is called once per frame
        void Update()
        {
            if (movementActive)
            {
                switch (movementMethod)
                {
                    case TransformationMethod.StaticTransformation:
                        StaticTransformationMovement();
                        break;
                    case TransformationMethod.TargetKeys:
                        TargetKeysMovement();
                        break;
                    case TransformationMethod.RandomizedTransformation:
                        RandomizedTransformationMovement();
                        break;
                }
                CurrentSpeedAndDirection();
                FaceTowardsMovementDirection();
            }

        }
    }
}
