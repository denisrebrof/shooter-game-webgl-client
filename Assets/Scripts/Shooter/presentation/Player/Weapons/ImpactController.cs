using System.Collections.Generic;
using UnityEngine;

namespace Shooter.presentation.Player.Weapons
{
    public class ImpactController : MonoBehaviour, IImpactNavigator
    {
        [SerializeField] private List<ImpactView> impactPool;

        private int index;

        private void Awake()
        {
            IImpactNavigator.Instance = this;
        }

        public void AddImpact(Vector3 position, Vector3 normal, ImpactType type)
        {
            index += 1;
            index %= impactPool.Count;
            impactPool[index].Play(position, normal, type);
        }
    }

    public interface IImpactNavigator
    {
        public static IImpactNavigator Instance;

        void AddImpact(Vector3 position, Vector3 normal, ImpactType type);
    }
}