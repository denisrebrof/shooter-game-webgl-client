using System.Collections.Generic;
using UnityEngine;

namespace Shooter.presentation.Player.Weapons
{
    public class ImpactController : MonoBehaviour, IImpactNavigator
    {
        [SerializeField] private List<ParticleSystem> impactPoolParticles;
        [SerializeField] private List<Transform> impactPoolTransforms;

        private int index;

        private void Awake()
        {
            IImpactNavigator.Instance = this;
        }

        public void AddImpact(Vector3 position, Vector3 normal)
        {
            index += 1;
            index %= impactPoolParticles.Count;
            var impactTransform = impactPoolTransforms[index];
            impactTransform.position = position;
            impactTransform.LookAt(position + normal);
            impactPoolParticles[index].Play();
        }
    }

    public interface IImpactNavigator
    {
        public static IImpactNavigator Instance;

        void AddImpact(Vector3 position, Vector3 normal);
    }
}