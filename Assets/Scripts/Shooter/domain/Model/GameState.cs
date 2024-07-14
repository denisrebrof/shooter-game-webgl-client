using System;
using System.Collections.Generic;

namespace Shooter.domain.Model
{
    [Serializable]
    public struct GameState
    {
        public int typeCode;
        public int playersHash;
        public List<PlayerData> playerData;
        public int winnerTeamId;
        public TeamData redTeamState;
        public TeamData blueTeamState;

        public GameStateTypes Type
        {
            get
            {
                if (Enum.IsDefined(typeof(GameStateTypes), typeCode))
                    return (GameStateTypes)typeCode;

                return GameStateTypes.Undefined;
            }
        }
        
        public Teams WinnerTeam
        {
            get
            {
                if (Enum.IsDefined(typeof(Teams), winnerTeamId))
                    return (Teams)winnerTeamId;

                return Teams.Undefined;
            }
        }
    }

    [Serializable]
    public struct TeamData
    {
        public int teamFlags;
        public int teamKills;
        public TransformSnapshot flagPos;
        public bool flagHasOwner;
        public long flagOwnerId;
    }

    [Serializable]
    public struct PlayerData
    {
        public long playerId;
        public int teamId;
        public int kills;
        public int death;
        public bool alive;
        public int hp;
        public TransformSnapshot pos;
        public float verticalLookAngle;
        public long selectedWeaponId;
        public bool crouching;
        public bool jumping;

        public Teams Team
        {
            get
            {
                if (Enum.IsDefined(typeof(Teams), teamId))
                    return (Teams)teamId;

                return Teams.Undefined;
            }
        }

        public bool IsBot => playerId < 0;
    }

    public enum GameStateTypes
    {
        Undefined = 0,
        Pending = 1,
        Playing = 2,
        Finished = 3
    }

    public enum Teams
    {
        Undefined = 0,
        Red = 1,
        Blue = 2,
    }
}