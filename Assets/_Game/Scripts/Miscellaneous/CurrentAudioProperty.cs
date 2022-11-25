namespace LearningByPlaying.AudioProperty
{
    public class CurrentAudioProperty
    {
        static string audioProperty = AudioProperties.None.ToString();
        public static string GetAudioProperty()
        {
            return audioProperty;
        }
        public static void SetAudioProperty(string value)
        {
            audioProperty = value;
        }
    }
}
