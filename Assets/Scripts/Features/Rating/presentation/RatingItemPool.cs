using UnityEngine;
using Utils.Pooling;

namespace Features.Rating.presentation
{
    public class RatingItemPool : MonoPool<RatingItemView>
    {
        [ContextMenu("Generate")]
        public void GeneratePool() => Generate();
    }
}