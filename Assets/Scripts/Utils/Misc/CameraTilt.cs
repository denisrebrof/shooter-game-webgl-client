using UnityEngine;

namespace Utils.Misc
{
    public class CameraTilt : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float lerpFactor = 1f;
        [SerializeField] private float yShift;
        [SerializeField] private float xShift;

        private Vector3 defaultAngles;

        private void Start()
        {
            defaultAngles = target.eulerAngles;
        }

        private void Reset() => target = transform;

        private void Update()
        {
            Vector2 mousePos = Input.mousePosition;
            var rot = defaultAngles + new Vector3(
                yShift * (mousePos.y / Screen.height - 0.5f),
                xShift * (mousePos.x / Screen.width - 0.5f),
                0f
            );
            var targetRot = Quaternion.Euler(rot);
            target.rotation = Quaternion.Slerp(target.rotation, targetRot, Time.deltaTime * lerpFactor);
        }
    }
}