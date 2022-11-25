namespace LearningByPlaying.GameType
{
    public class CurrentGameType
    {
        static string gameType = GameTypes.None.ToString();
        public static string GetGameType()
        {
            return gameType;
        }
        public static void SetGameType(string value)
        {
            gameType = value;
        }
    }
}
