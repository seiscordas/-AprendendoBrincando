using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneByName : MonoBehaviour
{
    public void LoadScene(string gameType)
    {
        if (gameType != null)
            SceneManager.LoadScene(gameType);
    }
}
