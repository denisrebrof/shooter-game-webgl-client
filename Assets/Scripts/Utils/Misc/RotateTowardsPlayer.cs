using UnityEngine;

namespace Utils.Misc
{
    public class RotateTowardsPlayer : MonoBehaviour
    {
        public bool zRotEnabled = false;

        //values that will be set in the Inspector
        private Transform Target;
        public float RotationSpeed;

        //values for internal use
        private Quaternion _lookRotation;
        private Vector3 _direction;
        
        private float playerHeight = 1.2f;

        private void Start()
        {
            Target = GameObject.FindWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
            //find the vector pointing from our position to the target
            _direction = (Target.position + Vector3.up * playerHeight - transform.position).normalized;
            if (!zRotEnabled)
                _direction = new Vector3(_direction.x, 0, _direction.z);

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
        }
    }
}