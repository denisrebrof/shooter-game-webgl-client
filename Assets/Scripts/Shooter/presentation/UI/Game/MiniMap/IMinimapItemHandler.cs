using UnityEngine;

namespace Shooter.presentation.UI.Game.MiniMap
{
    public interface IMinimapItemHandler
    {
        public void SetPlayer(Transform pos);
        public void Add(Transform pos, bool isEnemy);
        public void Remove(Transform pos, bool isEnemy);
    }
}