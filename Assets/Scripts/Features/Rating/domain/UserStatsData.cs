using System;
using System.Diagnostics.CodeAnalysis;

namespace Features.Rating.domain
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct UserStatsData
    {
        public int kills;
        public int death;
    }
}