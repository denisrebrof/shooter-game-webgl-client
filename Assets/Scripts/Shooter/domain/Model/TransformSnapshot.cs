using System;
using UnityEngine;

namespace Shooter.domain.Model
{
    [Serializable]
    public class TransformSnapshot
    {
        public float x;
        public float y;
        public float z;
        public float r;

        public Vector3 Pos => new(x, y, z);
    }

    public static class TransformSnapshotExtensions
    {
        public static TransformSnapshot GetSnapshot(this Transform transform)
        {
            var position = transform.position;
            return new TransformSnapshot
            {
                x = position.x,
                y = position.y,
                z = position.z,
                r = transform.rotation.eulerAngles.y
            };
        }
        
        public static TransformSnapshot GetSnapshot(this Vector3 position)
        {
            return new TransformSnapshot
            {
                x = position.x,
                y = position.y,
                z = position.z,
                r = 0
            };
        }
        
        public static void ApplySnapshot(this Transform transform, TransformSnapshot snapshot)
        {
            transform.position = new Vector3(snapshot.x, snapshot.y, snapshot.z);
            var newRotation = transform.rotation.eulerAngles;
            newRotation.y = snapshot.r;
            transform.rotation = Quaternion.Euler(newRotation);
        }
    }
}