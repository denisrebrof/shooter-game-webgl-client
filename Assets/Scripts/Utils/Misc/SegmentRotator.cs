using UnityEngine;

namespace Utils.Misc
{
    public class SegmentRotator : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private int segmentCount = 8;
        [SerializeField] private float delay = 0.5f;

        private int currentSector = 0;

        private float rotationTimer = -1f;

        private void OnEnable()
        {
            rotationTimer = delay;
            target.localRotation = Quaternion.identity;
            currentSector = 0;
        }

        private void Update()
        {
            rotationTimer -= Time.deltaTime;
            if (rotationTimer > 0)
                return;

            rotationTimer = delay;
            currentSector = (currentSector + 1) % segmentCount;
            var relativeRot = 1f - ((float)currentSector / segmentCount);
            var rotationVector = 360f * relativeRot * Vector3.forward;
            target.localRotation = Quaternion.Euler(rotationVector);
        }
    }
}