using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    Button button;
    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => Quit());
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(() => Quit());
    }

    public void Quit()
    {
        Application.Quit();
    }
}
