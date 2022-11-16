namespace LearningByPlaying.gameTheme
{
    public class CurrentGameTheme
    {
        static string gameTheme;
        public static string GetGameTheme()
        {
            return gameTheme;
        }
        public static void SetGameTheme(string value)
        {
            gameTheme = value;
        }
    }
}
