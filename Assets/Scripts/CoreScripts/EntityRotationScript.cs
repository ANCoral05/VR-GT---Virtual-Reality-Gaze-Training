using System.Collections.Generic;
using UnityEngine;

namespace VRK_BuildingBlocks
{
    public class EntityRotationScript : MonoBehaviour
    {
        public enum TransformationMethod
        {
            StaticTransformation,
            TargetKeys,
            RandomizedTransformation
        }

        public TransformationMethod method = TransformationMethod.StaticTransformation;

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

        [Header("Static Transform")]
        [Tooltip("Rotation angle of the movement.")]
        public Vector3 rotationAngle = Vector3.up;

        [Header("Target Key Transforms")]
        [Tooltip("The keys towards whose rotation angles the entity will rotate.")]
        public List<Vector3> targetKeys = new List<Vector3>();

        private int currentKeyIndex = 0;

        [Header("Randomized Transform")] // TODO
        [Tooltip("The step frequency at which the rotation is changed.")]

        [Tooltip("The maximum eulerAngle change per step.")]
        public Vector3 maxAngleChange = Vector3.one;



        public void StaticTransformation()
        {
            this.transform.eulerAngles += rotationAngle * currentRotationSpeed * Time.deltaTime;
        }

        public void TargetKeys()
        {
            if (targetKeys.Count == 0)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.Euler(targetKeys[currentKeyIndex]);

            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
            
            if (Quaternion.Angle(this.transform.rotation, targetRotation) < 0.1f)
            {
                this.transform.rotation = targetRotation;

                currentKeyIndex = (currentKeyIndex + 1) % targetKeys.Count;
            }
        }

        public void RandomizedTransformation()
        {
            this.transform.eulerAngles += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * currentRotationSpeed * Time.deltaTime;
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
