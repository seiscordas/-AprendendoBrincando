using LearningByPlaying.gameTheme;
using LearningByPlaying.GameType;
using TMPro;
using UnityEngine;

namespace LearningByPlaying
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        [SerializeField] private TextMeshProUGUI scoreText;

        int score = 0;
        int highscore = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            string key = CurrentGameType.GetGameType() + "_" + CurrentGameTheme.GetGameTheme();
            highscore = PlayerPrefsDataBase.Instance.ReadIntInfo(key);
            score = highscore;
            scoreText.text = score.ToString().PadLeft(5, '0');
        }

        public void AddPoint()
        {
            string key = CurrentGameType.GetGameType() + "_" + CurrentGameTheme.GetGameTheme();
            score++;
            scoreText.text = score.ToString().PadLeft(5, '0');
            if (score > highscore)
                PlayerPrefsDataBase.Instance.SaveIntInfo(key, score);
        }
    }
}
