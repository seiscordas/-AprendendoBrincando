using UnityEngine;
using UnityEngine.SceneManagement;

namespace LearningByPlaying
{
    public class LoadSceneHelper : MonoBehaviour
    {
        public void LoadScene(string scene)
        {
            if (scene != null)
                SceneManager.LoadScene(scene);
        }

        public void SetGameTheme(string gameTheme)
        {
            CurrentGameTheme.SetGameTheme(gameTheme);
            //PlayerPrefsDataBase.Instance.SaveStringInfo(PlayerPrefsDataBase.CURRENT_THEME, gameTheme);
        }
    }
}
