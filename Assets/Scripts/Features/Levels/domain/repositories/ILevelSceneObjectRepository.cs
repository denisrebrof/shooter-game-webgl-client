using UnityEngine;

namespace Features.Levels.domain.repositories
{
    public interface ILevelSceneObjectRepository
    {
        GameObject GetLevelScene(int levelId);
    }
}