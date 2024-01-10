using UnityEngine;

namespace Utils.Misc
{
    public class MoveForward : MonoBehaviour
    {
        private Transform target;
        public float speed;
    
        void Start()
        {
            target = GetComponent<Transform>();
        }
        void Update()
        {
            target.position += target.forward * speed * Time.deltaTime;
        }
    }
}
