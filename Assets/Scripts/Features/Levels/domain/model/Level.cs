namespace Features.Levels.domain.model
{
    public class Level
    {
        public readonly int ID;
        public readonly int Number;

        public Level(int id, int number)
        {
            ID = id;
            Number = number;
        }
    }
}