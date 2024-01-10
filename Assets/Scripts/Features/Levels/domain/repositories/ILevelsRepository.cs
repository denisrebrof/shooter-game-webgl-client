using System.Collections.Generic;
using Features.Levels.domain.model;

namespace Features.Levels.domain.repositories
{
    public interface ILevelsRepository
    {
        List<Level> GetLevels();
        Level GetLevel(int levelId);
    }
}