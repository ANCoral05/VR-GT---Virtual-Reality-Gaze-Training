using System.Collections.Generic;
using UnityEngine;

namespace VRK_BuildingBlocks
{
    public class EntityMovementScript : MonoBehaviour
    {
        public bool movementActive = true;

        public enum TransformationMethod
        {
            StaticTransformation,
            TargetKeys,
            RandomizedTransformation
        }
        public TransformationMethod movementMethod = TransformationMethod.StaticTransformation;

        public enum CurrentMovementState
        {
            Starting,
            Moving,
            Break
        }
        public CurrentMovementState currentMovementState;

        public class BreakEvent
        {
            public float breakTime;
            public Vector3 breakPosition;
            public Vector3 breakVelocity;

            public BreakEvent(float breakTime, Vector3 breakPosition, Vector3 breakVelocity)
            {
                this.breakTime = breakTime;
                this.breakPosition = breakPosition;
                this.breakVelocity = breakVelocity;
            }
        }
        private BreakEvent breakEvent = new BreakEvent(0, Vector3.zero, Vector3.zero);

        [Header("Movement")]
        [Tooltip("Speed which the entity aims to move at.")]
        public float targetSpeed = 1;

        [Tooltip("The acceleration of the entity. Set to -1 for infinite acceleration (instant targeted speed).")]
        public float movementAcceleration = -1;

        [Tooltip("The deceleration of the entity. Set to -1 for infinite deceleration (instant breaks).")]
        public float movementDampening = -1;

        [Tooltip("Maximum turn speed (in degree per second) of the entity. Set to -1 for infinite turn speed.")]
        public float maxTurnSpeed = -1;

        public float breakDuration = 0.75f;

        public float oscillationAmplitude = 0.1f;

        public float oscillationDuration = 0.5f;

        public float currentSpeed;

        public Vector3 currentDirection;

        public Transform movementTarget;

        [SerializeField, Tooltip("If activated, the target will face in moving direction.")]
        private bool faceTowardsMovementDirection = false;

        [Tooltip("The threshold for the difference between target speed and actual speed at which a 'break' maneuver will be executed. Set to -1 to disable this.")]
        public float velocityBreakThreshold = -1f;

        [Header("Static Transform")]
        [Tooltip("Targeted direction of the movement.")]
        public Vector3 targetDirection = Vector3.forward;

        [Header("Target Key Transforms")]
        [Tooltip("The keys towards which the entity will move.")]
        public List<Transform> targetKeys = new List<Transform>();

        [Tooltip("If activated, the next target location will be selected randomly from the list each time.")]
        public bool randomizeTargetKeys;

        [Tooltip("Set a fixed travel time between the target keys which will overwrite the speed variable. Set to -1 for no fixed travel time.")]
        public float fixedTravelTime = -1;

        [Tooltip("The maximum number of steps the entity will take before stopping. Set to -1 for infinite steps.")]
        public int maxStepNumber = -1;

        private float targetSpeedBackup;

        private int currentKeyIndex = 0;

        private bool targetReached;

        private void Awake()
        {
            if(movementTarget == null)
            {
                movementTarget = new GameObject(this.gameObject.name + "_MovementTarget").transform;
                movementTarget.position = this.transform.position;
            }
        }

        public void StaticTransformationMovement()
        {
            this.transform.position += currentDirection.normalized * currentSpeed * Time.deltaTime;
        }

        public void SetNewTargetCourse(Transform movementTarget)
        {
            if (fixedTravelTime < 0)
            {
                targetSpeed = targetSpeed;
            }
            else if (fixedTravelTime > 0)
            {
                float distanceToTarget = (movementTarget.position - this.transform.position).magnitude;

                targetSpeed = distanceToTarget / fixedTravelTime;
            }
        }

        public void TargetMovement()
        {
            targetDirection = movementTarget.position - this.transform.position;

            float distanceToTarget = (movementTarget.position - this.transform.position).magnitude;

            if (distanceToTarget <= 0.01f || distanceToTarget < currentSpeed * Time.deltaTime)
            {
                GoalReached();
            }
        }

        public void GoalReached()
        {
            MovementBreak();

            movementTarget = TargetKeysSelection();
        }

        public Transform TargetKeysSelection()
        {
            if (targetKeys.Count == 0)
            {
                return null;
            }

            this.transform.position = movementTarget.position;

            if (randomizeTargetKeys)
            {
                currentKeyIndex += Random.Range(1, targetKeys.Count);
            }
            else
            {
                currentKeyIndex++;
            }
            currentKeyIndex %= targetKeys.Count;

            return targetKeys[currentKeyIndex];
        }
        

        public void CalculateCurrentSpeedAndDirection()
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
                    if (currentSpeed - targetSpeed >= velocityBreakThreshold)
                    {
                        MovementBreak();
                    }
                    else
                    {
                        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, movementDampening * Time.deltaTime);
                    }
                }
            }

            if (!targetReached && currentDirection != targetDirection)
            {
                if (currentDirection == Vector3.zero)
                {
                    currentDirection = targetDirection;
                }
                else if (maxTurnSpeed < 0)
                {
                    currentDirection = targetDirection;
                }
                else
                {
                    currentDirection = Vector3.RotateTowards(currentDirection, targetDirection, maxTurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 0);
                }
            }

            // move the entity
            this.transform.position += currentDirection.normalized * currentSpeed * Time.deltaTime;

            if (faceTowardsMovementDirection)
            {
                this.transform.rotation = Quaternion.LookRotation(currentDirection);
            }
        }

        public void MovementStart()
        {
            currentMovementState = CurrentMovementState.Moving;
        }

        public void MovementBreak()
        {
            currentMovementState = CurrentMovementState.Break;

            breakEvent.breakTime = Time.time;
            breakEvent.breakPosition = this.transform.position;
            breakEvent.breakVelocity = currentDirection.normalized * currentSpeed;

            currentSpeed = 0;
        }

        public void Stopped()
        {
            Oscillation(breakEvent, oscillationAmplitude, oscillationDuration);

            if(Time.time >= breakEvent.breakTime + breakDuration)
            {
                currentMovementState = CurrentMovementState.Starting;
            }
        }

        public void RandomizedTransformationMovement()
        {
            
        }

        public void Oscillation(BreakEvent breakEvent, float maxAmplitude, float duration)
        {
            float elapsedTime = Time.time - breakEvent.breakTime;

            if (elapsedTime < duration)
            {
                float oscillation = maxAmplitude * Mathf.Sin(elapsedTime * (breakEvent.breakVelocity.magnitude / maxAmplitude)) * (1 - elapsedTime / duration);
                
                this.transform.position = breakEvent.breakPosition + breakEvent.breakVelocity.normalized * oscillation;
            }
            else
            {
                this.transform.position = breakEvent.breakPosition;
            }
        }

        void Update()
        {
            if (currentMovementState == CurrentMovementState.Starting)
            {
                MovementStart();
            }

            if (movementActive && currentMovementState == CurrentMovementState.Moving)
            {
                switch (movementMethod)
                {
                    case TransformationMethod.StaticTransformation:
                        StaticTransformationMovement();
                        break;
                    case TransformationMethod.TargetKeys:
                        TargetMovement();
                        break;
                    case TransformationMethod.RandomizedTransformation:
                        RandomizedTransformationMovement();
                        break;
                }
                CalculateCurrentSpeedAndDirection();
            }

            if (currentMovementState == CurrentMovementState.Break)
            {
                Stopped();
            }
        }
    }
}