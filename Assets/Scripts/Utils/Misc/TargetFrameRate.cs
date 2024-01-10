using UnityEngine;

namespace Utils.Misc
{
    public class TargetFrameRate: MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}