namespace Utils.WebSocketClient.domain.model
{
    public static class Commands
    {
        public const long LogIn = 0L;

        public const long Userdata = 1L;

        public const long LobbyAction = 2L;
        public const long LobbyState = 3L;
        public const long MatchState = 4L;

        public const long Balance = 5L;

        public const long Ping = 7L;

        public const long GameState = 6L;

        public const long ActionShoot = 9L;
        public const long ActionHit = 10L;

        public const long IntentSubmitPosition = 11L;
        public const long IntentSelectWeapon = 12L;
        public const long IntentShoot = 13L;
        public const long IntentHit = 14L;

        public const long LeaveMatch = 15L;
        public const long JoinMatch = 16L;

        public const long TimeLeft = 17L;
        public const long GetGames = 18L;

        public const long ActionJoinedStateChange = 19L;

        public const long GetUsername = 20L;
        public const long SetUsername = 21L;
        
        public const long Rating = 22L;
        public const long UserStats = 23L;
        public const long ResVersion = 24L;
    }
}