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
            highscore = PlayerPrefsDataBase.Instance.ReadIntInfo(GameType.GetGameType());
            score = highscore;
            scoreText.text = score.ToString();
        }

        public void AddPoint()
        {
            score++;
            scoreText.text = score.ToString();
            if (score > highscore)
                PlayerPrefsDataBase.Instance.SaveIntInfo(GameType.GetGameType(), score);
            //PlayerPrefs.SetInt(nameof(highscore), score);
        }
    }
}
