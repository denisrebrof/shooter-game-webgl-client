using UnityEngine;
using Utils.Pooling;

namespace Shooter.presentation.UI.Lobby.Store
{
    public class StoreItemPool : MonoPool<StoreItemView>
    {
        [ContextMenu("Generate")]
        public void GeneratePool() => Generate();
    }
}