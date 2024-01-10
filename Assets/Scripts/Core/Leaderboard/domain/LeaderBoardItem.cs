namespace Core.Leaderboard.domain
{
    public class LeaderBoardItem
    {
        public string PlayerId;
        public string PlayerName;
        public int Position;
        public int Score;

        public LeaderBoardItem(string playerName, int position, int score, string playerId)
        {
            PlayerName = playerName;
            Position = position;
            Score = score;
            PlayerId = playerId;
        }
    }
}