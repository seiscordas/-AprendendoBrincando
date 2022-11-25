namespace LearningByPlaying.gameTheme
{
    public class CurrentGameTheme
    {
        static string gameTheme = GameThemes.None.ToString();
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
