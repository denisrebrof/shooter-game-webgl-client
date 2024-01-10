using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Utils.Misc
{
    public class LightProbesSceneSetup : MonoBehaviour
    {
        [CanBeNull] private Coroutine tetrahedralizeCoroutine;

        void Start()
        {
            tetrahedralizeCoroutine = StartCoroutine(Tetrahedralize());
        }

        private static IEnumerator Tetrahedralize()
        {
            yield return new WaitForSeconds(0.1f);
            LightProbes.Tetrahedralize();
        }

        private void OnDestroy()
        {
            if (tetrahedralizeCoroutine != null)
            {
                StopCoroutine(tetrahedralizeCoroutine);
            }
        }
    }
}