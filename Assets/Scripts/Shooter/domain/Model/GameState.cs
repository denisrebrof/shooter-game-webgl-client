using System;
using System.Collections.Generic;

namespace Shooter.domain.Model
{
    [Serializable]
    public struct GameState
    {
        public int typeCode;
        public List<PlayerData> playerData;
        public int winnerTeamId;
        public int redTeamKills;
        public int blueTeamKills;

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

        public Teams Team
        {
            get
            {
                if (Enum.IsDefined(typeof(Teams), teamId))
                    return (Teams)teamId;

                return Teams.Undefined;
            }
        }
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