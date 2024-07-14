using UnityEngine;

namespace Shooter.presentation.Player.Weapons
{
    public class ImpactView : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private ParticleSystem bloodSystem;
        [SerializeField] private ParticleSystem metalSystem;
        [SerializeField] private ParticleSystem concreteSystem;
        [SerializeField] private ParticleSystem dirtSystem;

        public void Play(Vector3 pos, Vector3 norm, ImpactType type)
        {
            target.position = pos;
            target.LookAt(pos + norm);
            Stop();
            GetSystem(type).Play();
        }

        private void Stop()
        {
            bloodSystem.Stop();
            metalSystem.Stop();
            concreteSystem.Stop();
            dirtSystem.Stop();
            bloodSystem.Clear();
            metalSystem.Clear();
            concreteSystem.Clear();
            dirtSystem.Clear();
        }

        private ParticleSystem GetSystem(ImpactType type) =>
            type switch
            {
                ImpactType.Blood => bloodSystem,
                ImpactType.Metal => metalSystem,
                ImpactType.Concrete => concreteSystem,
                ImpactType.Dirt => dirtSystem,
                _ => GetSystem(ImpactType.Metal)
            };
    }

    public enum ImpactType
    {
        Blood,
        Metal,
        Concrete,
        Dirt
    }

    public static class ImpactUtils
    {
        public static ImpactType GetImpactType(Collider collider)
        {
            if (collider.CompareTag("Concrete"))
                return ImpactType.Concrete;

            if (collider.CompareTag("Player") || collider.CompareTag("OtherPlayer"))
                return ImpactType.Blood;

            if (collider.CompareTag("Dirt"))
                return ImpactType.Dirt;

            return ImpactType.Metal;
        }
    }
}