using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LearningByPlaying.WordWriterSystem
{
    public class PlayCharSound : MonoBehaviour
    {
        AudioSource audioSource;
        Button button;

        private void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            PlaySount();
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(() => Play());
        }

        private void PlaySount()
        {
            string charText = GetComponentInChildren<TextMeshProUGUI>().text;
            audioSource.clip = LoadAudio("Alphabet/" + charText);
            audioSource.Play();
            StartListenButton();
        }

        private AudioClip LoadAudio(string path)
        {
            return Resources.Load<AudioClip>("Audios/" + path);
        }

        private void StartListenButton()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => Play());
        }

        private void Play()
        {
            audioSource.Play();
        }
    }
}
