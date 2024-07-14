using UnityEngine;
using Utils.Pooling;

namespace Features.Lobby.presentation
{
    public class GameListItemControllerPool : MonoPool<GameListItemController>
    {
        [ContextMenu("Generate")]
        public void GeneratePool() => Generate();
    }
}