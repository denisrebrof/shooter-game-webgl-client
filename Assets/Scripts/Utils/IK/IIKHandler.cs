using UnityEngine;

namespace Utils.IK
{
    public interface IIKHandler
    {
        public void SetTargets(Transform l, Transform r);
    }
}