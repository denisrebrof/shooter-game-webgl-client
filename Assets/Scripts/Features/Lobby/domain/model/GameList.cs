using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Features.Lobby.domain.model
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct GameList
    {
        public List<GameListItemData> matches;
    }

    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct GameListItemData
    {
        public string matchId;
        public int currentParticipants;
        public int maxParticipants;
        public long stateCode;
    }
}