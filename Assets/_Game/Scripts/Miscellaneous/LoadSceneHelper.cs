using LearningByPlaying.gameTheme;
using LearningByPlaying.GameType;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LearningByPlaying
{
    public class LoadSceneHelper : MonoBehaviour, ISerializationCallbackReceiver
    {
        public static List<string> TMPList;

        private List<string> popupList;
        private Button button;

        [ListToPopup(typeof(LoadSceneHelper), nameof(TMPList))]
        [Header("Select Scene")]
        [SerializeField] private string sceneName;
        [Header("Select Game Themes")]
        [SerializeField] GameThemes gameTheme = new();
        [Header("Select Game Types")]
        [SerializeField] GameTypes gameType = new();


        public void OnBeforeSerialize()
        {
            popupList = GetAllScenesInBuild();
            TMPList = popupList;
        }

        private void OnEnable()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => SetGameTheme(gameTheme.ToString()));
            button.onClick.AddListener(() => LoadScene(sceneName));
        }

        private void OnDisable()
        {
            button.onClick.RemoveAllListeners();
        }

        private List<string> GetAllScenesInBuild()
        {
            List<string> allScene = new List<string>();

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                allScene.Add(sceneName);
            }
            return allScene;
        }

        private void LoadScene(string scene)
        {
            if (scene != null)
                SceneManager.LoadScene(scene);
        }

        private void SetGameTheme(string gameTheme)
        {
            CurrentGameTheme.SetGameTheme(gameTheme);
        }

        private void SetGameType(string gameType)
        {
            CurrentGameTheme.SetGameTheme(gameType);
        }

        public void OnAfterDeserialize() { }
    }
}
